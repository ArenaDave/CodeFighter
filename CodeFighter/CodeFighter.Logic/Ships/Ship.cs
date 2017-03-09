using CodeFighter.Logic.Parts;
using CodeFighter.Logic.Players;
using CodeFighter.Logic.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeFighter.Logic.Ships
{
    public class ShipEventArgs : EventArgs
    {
        public Ship Ship { get; set; }

        public ShipEventArgs(Ship ship)
        {
            this.Ship = ship;
        }
    }

    public delegate void ShipDestroyedEvent(object sender, ShipEventArgs e);

    public class Ship
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
        public StatWithMax MP { get; set; }
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
        #endregion

        public dynamic GameLogic;

        public override string ToString()
        {
            if (this.Name != string.Empty)
                return string.Format("{0} ({1}-class {2}) (HP:{3}/{4})",
                    this.Name,
                    this.Hull.ClassName,
                    this.Hull.Size.Name,
                    this.HP.Current,
                    this.HP.Max);
            else
                return string.Format("{0}-class {1} (HP:{2}/{3})",
                    this.Hull.ClassName,
                    this.Hull.Size.Name,
                    this.HP.Current,
                    this.HP.Max);
        }

        #region Events
        public event ShipDestroyedEvent OnShipDestroyed;
        #endregion

        #region Public Methods
        /// <summary>
        /// Handles incoming damage to the Ship through any equipped DefenseParts
        /// </summary>
        /// <param name="Damage">Amount of damage to take</param>
        /// <returns>List of status results from DefenseParts</returns>
        public List<string> HitFor(int Damage)
        {
            return HitFor(Damage, string.Empty);
        }

        /// <summary>
        /// Handles incoming damage to the Ship through any equipped DefenseParts, including resistance based on damage type
        /// </summary>
        /// <param name="Damage">Amount of damage to take</param>
        /// <param name="DamageType">Type of damage to take (Data Driven)</param>
        /// <returns>List of status results from DefenseParts</returns>
        public List<string> HitFor(int Damage, string DamageType)
        {
            List<string> result = new List<string>();
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
                    OnShipDestroyed?.Invoke(this, new ShipEventArgs(this));
                    IsDestroyed = true;
                }
            }

            return result;
        }

        #endregion

    }
}
