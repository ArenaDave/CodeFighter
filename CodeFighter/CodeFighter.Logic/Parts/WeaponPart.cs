using CodeFighter.Logic.Actions;
using CodeFighter.Logic.Ships;
using CodeFighter.Logic.Utility;
using System.Collections.Generic;
using System.Linq;

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
        public List<string> Fire()
        {
            return Fire(11, 96); // 10% miss, 5% crit
        }
        /// <summary>
        /// Fires the weapon at its existing target with specific hit/crit chances
        /// </summary>
        /// <param name="hitAbove">Minimum number needed (out of 100) to hit the target</param>
        /// <param name="critAbove">Minimum number needed (out of 100) to critically-hit the target</param>
        /// <returns>Status result</returns>
        public List<string> Fire(int hitAbove, int critAbove)
        {
            Ship targetShip = (Ship)Target;
            List<string> result = new List<string>();
            currentReload = ReloadTime;
            using (RNG rng = new RNG())
            {
                int hitNum = rng.d100();

                if (hitNum >= critAbove)
                {
                    result.Add(string.Format("{0} CRITS {1} for {2}",
                        this.Name,
                        targetShip.Name,
                        this.WeaponDamage
                        ));
                    result = result.Concat(targetShip.HitFor(WeaponDamage * CritMultiplier, DamageType)).ToList<string>();
                }
                else if (hitNum >= hitAbove)
                {
                    result.Add(string.Format("{0} hits {1} for {2}",
                        this.Name,
                        targetShip.Name,
                        this.WeaponDamage
                        ));
                    result = result.Concat(targetShip.HitFor(WeaponDamage, DamageType)).ToList<string>();
                }
                else
                    result.Add(string.Format("{0} Missed!", this.Name));
            }

            return result;
        }

        /// <summary>
        /// Reduces the reload counter for the weapon
        /// </summary>
        /// <returns>Number of turns remaining until the weapon is reloaded</returns>
        public int Reload()
        {
            if (currentReload > 0)
                return currentReload--;
            else
                return 0;
        }

        #endregion

        #region Constructors
        public WeaponPart(string name, int maxHP, double mass, List<BaseAction> actions, int damage, double range, string damageType, string firingType, int critMultiplier, int reloadTime)
            : base(name, maxHP, mass, actions)
        {
            this.WeaponDamage = damage;
            this.Range = range;
            this.FiringType = firingType;
            this.CritMultiplier = critMultiplier;
            this.ReloadTime = reloadTime;
        }

        #endregion



    }
}
