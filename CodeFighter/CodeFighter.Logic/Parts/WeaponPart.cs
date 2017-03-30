using CodeFighter.Logic.Actions;
using CodeFighter.Logic.Ships;
using CodeFighter.Logic.Utility;
using System.Collections.Generic;
using System.Linq;
using System;

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
            return string.Format("{0} ({1}{2}DMG: {3}{4}, {5}x on Crit, {6} Reload)",
                this.Name,
                (FiringType != string.Empty ? string.Format("{0} ", FiringType) : string.Empty),
                (Range > 0d ? string.Format("Rng: {0} ", Range.ToString()) : string.Empty),
                WeaponDamage.ToString(),
                (DamageType != string.Empty ? string.Format(" - ({0})", DamageType) : string.Empty),
                CritMultiplier.ToString(),
                (currentReload > 0 ? string.Format("{0}/{1}", currentReload, ReloadTime) : ReloadTime.ToString())
                );
        }

        /// <summary>
        /// Fires the weapon at its existing target with a fixed 90% hit 5% crit chance
        /// </summary>
        /// <returns>Status result</returns>
        public AttackResult Fire()
        {
            return Fire(11, 96); // 10% miss, 5% crit
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
            copy.partID = this.partID;
            copy.Name = (string)this.Name.Clone();
            copy.HP = (StatWithMax)this.HP.Clone();
            copy.IsDestroyed = this.IsDestroyed;
            //copy.Target = (Ship)this.Target.Clone();
            copy.Mass = this.Mass;
            copy.Actions = (List<BaseAction>)this.Actions?.Clone();
            copy.currentReload = this.currentReload;
            copy.WeaponDamage = this.WeaponDamage;
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
        public WeaponPart(string name, int maxHP, double mass, List<BaseAction> actions, int damage, double range, string damageType, string firingType, int critMultiplier, int reloadTime, bool pointDefense)
            : base(name, maxHP, mass, actions)
        {
            this.WeaponDamage = damage;
            this.Range = range;
            this.FiringType = firingType;
            this.CritMultiplier = critMultiplier;
            this.ReloadTime = reloadTime;
            this.IsPointDefense = pointDefense;
        }
        private WeaponPart() : base() { }
        #endregion



    }
}
