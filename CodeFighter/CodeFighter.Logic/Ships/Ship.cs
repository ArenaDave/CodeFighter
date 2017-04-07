﻿using CodeFighter.Data;
using CodeFighter.Logic.Animations;
using CodeFighter.Logic.Parts;
using CodeFighter.Logic.Players;
using CodeFighter.Logic.Simulations;
using CodeFighter.Logic.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeFighter.Logic.Ships
{
    public class ShipEventArgs : MessageEventArgs
    {
        public int ShipID { get; set; }

        public ShipEventArgs(int shipId, List<string> messages)
            : base(messages)
        {
            this.ShipID = shipId;
        }
    }

    public delegate void ShipDestroyedEvent(object sender, ShipEventArgs e);

    public class Ship : ICloneable
    {
        #region Properties
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsDestroyed { get; set; }
        public Player Owner { get; set; }
        public List<BasePart> Parts { get; set; }
        public Point Position { get; set; }
        public ShipHull Hull { get; set; }
        public StatWithMax HP { get { return this.Hull.HullPoints; } set { this.Hull.HullPoints = value; } }
        public StatWithMax MP { get; internal set; }
        public double TotalMass
        {
            get
            {
                // mass of hull and all parts
                double mass = Hull.Size.Mass;
                foreach (var part in Parts)
                {
                    if (part.IsDestroyed)
                        mass += part.Mass * 0.25; // destroyed parts only count 25% of their mass
                    else
                        mass += part.Mass;
                }
                return mass;
            }
        }
        public int MaxMP
        {
            get
            {
                // diminishing returns function of mass versus thrust
                double totalThrust = 0;
                if (Parts != null)
                    foreach (EnginePart part in Parts.Where(x => x is EnginePart))
                    {
                        totalThrust += part.Thrust;
                    }
                if (totalThrust <= 0)
                    return 0;

                double sizeCategory = Math.Log((TotalMass / 50), 4);
                double engineCount = totalThrust / 100;
                double MP = (-0.0332 * sizeCategory + 0.405) * engineCount + (-0.29093 * sizeCategory + 1.7867);

                return Convert.ToInt32(Math.Round(MP));
            }
        }
        // not sure how to calculate this yet
        public int Initiative { get { return 1; } }
        #endregion

        #region Constructor
        internal Ship()
        {

        }
        public Ship(int id, string name, Player owner, ShipHull hull, List<BasePart> parts, Point originPosition)
        {
            this.ID = id;
            this.Name = name;
            this.Owner = owner;
            this.Hull = hull;
            this.Parts = parts;
            this.Position = originPosition;

            this.IsDestroyed = false;
            this.MP = new StatWithMax(MaxMP);
        }
        
        #endregion

        public IGameLogic GameLogic { get; internal set; }

        public override string ToString()
        {
            if (this.Name != string.Empty)
                return string.Format("{0} ({1}-class {2}) (HP:{3}/{4}){5}",
                    this.Name,
                    this.Hull.ClassName,
                    this.Hull.Size.Name,
                    this.HP.Current,
                    this.HP.Max,
                    this.IsDestroyed?" [DESTROYED!]":"");
            else
                return string.Format("{0}-class {1} (HP:{2}/{3}){4}",
                    this.Hull.ClassName,
                    this.Hull.Size.Name,
                    this.HP.Current,
                    this.HP.Max,
                    this.IsDestroyed ? " [DESTROYED!]" : "");
        }

        #region Public Methods
        /// <summary>
        /// Handles incoming damage to the Ship through any equipped DefenseParts, including resistance based on damage type
        /// </summary>
        /// <param name="Damage">Amount of damage to take</param>
        /// <param name="DamageType">Type of damage to take (Data Driven)</param>
        /// <returns>List of status results from DefenseParts</returns>
        public List<string> HitFor(int Damage, string DamageType, out bool isDestroyed)
        {
            List<string> result = new List<string>();
            isDestroyed = false;
            // loop through all defense parts and apply the damage to each
            foreach (DefensePart defense in Parts.Where(f => f is DefensePart && !f.IsDestroyed))
            {
                DefenseResult res = defense.TakeHit(Damage);
                Damage = res.Remainder;
                foreach (string message in res.Messages)
                    result.Add(string.Format(" ==> {0}", message));
                if (Damage <= 0)
                    break;
            }
            // if there is any damage that got past the defense parts...
            if (Damage > 0)
            {
                // decrease current HP
                HP.Current -= Damage;

                result.Add(string.Format("{0} Damage made it through!", Damage));

                // chance to destroy ship part
                using (RNG rand = new RNG())
                {
                    // 50% chance of a part being affected
                    int countOfEquipment = Parts.Count * 2;
                    int random = rand.d(countOfEquipment);
                    if (random <= Parts.Count)
                    {
                        // only matters if the selected part isn't already destroyed
                        if (!Parts[random - 1].IsDestroyed)
                        {
                            Parts[random - 1].IsDestroyed = true;
                            result.Add(string.Format("{0} is destroyed by the damage!", Parts[random - 1].Name));
                        }
                    }

                }
                // oh no, we died!
                if (HP.Current <= 0)
                {
                    isDestroyed = true;
                    IsDestroyed = true;
                }
            }
            
            return result;
        }

        public object Clone()
        {
            Ship copy = new Ship();
            copy.ID = this.ID;
            copy.Name = (string)this.Name.Clone();
            copy.IsDestroyed = this.IsDestroyed;
            copy.Owner = (Player)this.Owner.Clone();
            copy.Parts = (List<BasePart>)Parts.Clone();
            copy.Position = this.Position;
            copy.Hull = (ShipHull)this.Hull.Clone();
            copy.MP = (StatWithMax)this.MP.Clone();
            return copy;
        }

        public Animation EndOfTurn()
        {
            List<string> resultMessages = new List<string>();
            resultMessages.Add(string.Format("Resolving End-of-Turn for {0}", this.ToString()));

            this.MP.Max = this.MaxMP;
            this.MP.Current = this.MP.Max;

            //Process Actions
            foreach (BasePart part in Parts.Where(x => !x.IsDestroyed))
            {
                resultMessages.AddRange(part.DoAction(this));
            }

            // clean up weapons
            foreach (WeaponPart weapon in Parts.Where(x => x is WeaponPart && !x.IsDestroyed))
            {
                if (weapon.ReloadTime > 0)
                    resultMessages.Add(string.Format("{0} will be reloaded in {1} turns", weapon.Name, weapon.Reload()));
                weapon.EndOfTurn();
            }

            return new Animation(AnimationActionType.Message, null, resultMessages);


        }
        #endregion

    }
}
