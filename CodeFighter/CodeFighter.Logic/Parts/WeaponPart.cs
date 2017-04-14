using CodeFighter.Logic.Actions;
using CodeFighter.Logic.Ships;
using CodeFighter.Logic.Utility;
using System.Collections.Generic;
using System.Linq;
using System;
using CodeFighter.Data;

namespace CodeFighter.Logic.Parts
{
    public class WeaponPart : BasePart
    {

        #region Private Variables
        int currentReload = 0;
        #endregion

        #region Public Properties
        public bool IsLoaded
        {
            get
            {
                if (currentReload == 0)
                    return true;
                else
                    return false;
            }
        }
        public int WeaponDamage { get; private set; }
        public int CritChance { get; private set; }
        public int CritMultiplier { get; private set; }
        public int ReloadTime { get; private set; }
        public string DamageType { get; private set; }
        public string FiringType { get; private set; }
        public double Range { get; private set; }
        public bool IsPointDefense { get; private set; }
        public bool HasFiredThisRound { get; private set; }
        #endregion

        #region Public Methods

        public override string ToString()
        {
            return string.Format("{0} ({1}{2}DMG: {3}{4}, {5}x on Crit{6}){7}",
                this.Name,
                (FiringType != string.Empty ? string.Format("{0} ", FiringType) : string.Empty),
                (Range > 0d ? string.Format("Rng: {0} ", Range.ToString()) : string.Empty),
                WeaponDamage.ToString(),
                (DamageType != string.Empty ? string.Format(" - ({0})", DamageType) : string.Empty),
                CritMultiplier.ToString(),
                ReloadTime > 0 ? (currentReload > 0 ? string.Format(", Reloading: {0}/{1}", currentReload, ReloadTime) : string.Format(", {0} Reload", ReloadTime.ToString())) : "",
                IsDestroyed ? " [DESTROYED!]" : ""
                );
        }

        /// <summary>
        /// Fires the weapon at its existing target with a fixed 90% hit 5% crit chance
        /// </summary>
        /// <returns>Status result</returns>
        public AttackResult Fire()
        {
            return Fire(11, 100 - CritChance); // 10% miss, 5% crit
        }
        /// <summary>
        /// Fires the weapon at its existing target with specific hit/crit chances
        /// </summary>
        /// <param name="hitAbove">Minimum number needed (out of 100) to hit the target</param>
        /// <param name="critAbove">Minimum number needed (out of 100) to critically-hit the target</param>
        /// <returns>Status result</returns>
        public AttackResult Fire(int hitAbove, int critAbove)
        {
            Ship targetShip = (Ship)Target;
            AttackResult result = new AttackResult();
            List<string> messages = new List<string>();
            currentReload = ReloadTime;
            using (RNG rng = new RNG())
            {
                int hitNum = rng.d100();

                if (hitNum >= critAbove)
                {
                    messages.Add(string.Format("{0} CRITS {1} for {2}",
                        this.Name,
                        targetShip.Name,
                        this.WeaponDamage * this.CritMultiplier
                        ));
                    messages = messages.Concat(targetShip.HitFor(WeaponDamage * CritMultiplier, DamageType, out result.TargetDestroyed)).ToList<string>();
                    result.IsHit = true;
                    result.IsCrit = true;
                }
                else if (hitNum >= hitAbove)
                {
                    messages.Add(string.Format("{0} hits {1} for {2}",
                        this.Name,
                        targetShip.Name,
                        this.WeaponDamage
                        ));
                    messages = messages.Concat(targetShip.HitFor(WeaponDamage, DamageType, out result.TargetDestroyed)).ToList<string>();
                    result.IsCrit = false;
                    result.IsHit = true;
                }
                else
                {
                    messages.Add(string.Format("{0} Missed!", this.Name));
                    result.IsHit = false;
                    result.IsCrit = false;
                }
            }
            this.HasFiredThisRound = true;

            result.Messages = messages;
            return result;
        }

        /// <summary>
        /// Reduces the reload counter for the weapon
        /// </summary>
        /// <returns>Number of turns remaining until the weapon is reloaded</returns>
        internal int Reload()
        {
            if (currentReload > 0)
                return currentReload--;
            else
                return 0;
        }

        internal void EndOfTurn()
        {
            this.HasFiredThisRound = false;
        }

        public override object Clone()
        {
            WeaponPart copy = new WeaponPart();
            copy.Name = (string)this.Name.Clone();
            copy.HP = (StatWithMax)this.HP.Clone();
            copy.IsDestroyed = this.IsDestroyed;
            copy.Mass = this.Mass;
            copy.Actions = (List<BaseAction>)this.Actions?.Clone();
            copy.currentReload = this.currentReload;
            copy.WeaponDamage = this.WeaponDamage;
            copy.CritChance = this.CritChance;
            copy.CritMultiplier = this.CritMultiplier;
            copy.ReloadTime = this.ReloadTime;
            copy.DamageType = (string)this.DamageType?.Clone();
            copy.FiringType = (string)this.FiringType?.Clone();
            copy.Range = this.Range;
            copy.IsPointDefense = this.IsPointDefense;
            copy.HasFiredThisRound = this.HasFiredThisRound;
            return copy;
        }

        #endregion

        #region Constructors
        private WeaponPart() : base() { }
        public WeaponPart(PartData data, KeelClassification classification, List<BaseAction> actions)
            : base(data, classification, actions)
        {
            baseStatsFromFiringType(data.FiringType, classification);
            modStatsFromDamageType(data.DamageType);
        }
        #endregion

        void baseStatsFromFiringType(string firingType, KeelClassification classification)
        {
            this.FiringType = firingType;
            if (firingType == Utility.FiringType.Beam)
            {
                this.WeaponDamage = Convert.ToInt32(Math.Round(5 * classification.PartFactor));
                this.ReloadTime = 0;
                this.CritChance = 5;
                this.CritMultiplier = 2;
                this.Range = 3;
            }
            else if (firingType == Utility.FiringType.Cannon)
            {
                this.WeaponDamage = Convert.ToInt32(Math.Round(10 * classification.PartFactor));
                this.ReloadTime = 1;
                this.CritChance = 10;
                this.CritMultiplier = 3;
                this.Range = 4;
            }
            else if (firingType == Utility.FiringType.Launcher)
            {
                this.WeaponDamage = Convert.ToInt32(Math.Round(15 * classification.PartFactor));
                this.ReloadTime = 2;
                this.CritChance = 5;
                this.CritMultiplier = 2;
                this.Range = 5;
            }
            this.IsPointDefense = false;
        }

        void modStatsFromDamageType(string damageType)
        {
            this.DamageType = damageType;
            if (damageType == Utility.DamageType.Energy)
            {
                this.WeaponDamage += 1;
                this.Range -= 1;
                this.IsPointDefense = true;
            }
            else if (damageType == Utility.DamageType.Plasma)
            {
                this.ReloadTime += 1;
                this.CritMultiplier += 2;
                this.CritChance += 10;
            }
            else if (damageType == Utility.DamageType.Explosive)
            {
                this.WeaponDamage -= 1;
                this.Range += 1;
            }
            else if (damageType == Utility.DamageType.Kinetic)
            {
                this.WeaponDamage += 4;
                this.ReloadTime -= 1;
                this.CritChance = 0;
                this.Range -= 2;
            }
        }
    }
}
