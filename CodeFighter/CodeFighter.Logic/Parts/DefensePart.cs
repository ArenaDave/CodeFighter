using CodeFighter.Logic.Actions;
using CodeFighter.Logic.Utility;
using System.Collections.Generic;
using System;
using CodeFighter.Logic.Ships;
using CodeFighter.Data;

namespace CodeFighter.Logic.Parts
{
    public class DefensePart : BasePart
    {
        #region Public Properties
        public int DR { get; private set; }
        public int DRVsEnergy { get; private set; }
        public int DRVsPlasma { get; private set; }
        public int DRVsExplosive { get; private set; }
        public int DRVsKinetic { get; private set; }
        public string DownAdjective { get; private set; }
        public string PenetrateVerb { get; private set; }
        #endregion

        #region Private Methods
        #endregion

        #region Public Methods
        public override string ToString()
        {
            return string.Format("{0} (DR:{1}) (HP:{2}/{3}){4}", this.Name, string.Format("{0}/{1}/{2}/{3}",DRVsEnergy,DRVsPlasma,DRVsExplosive,DRVsKinetic), HP.Current.ToString(), HP.Max.ToString(), this.IsDestroyed ? " [DESTROYED!]" : "");
        }
        /// <summary>
        /// Handles incoming damage
        /// </summary>
        /// <param name="Damage">Amount of incoming damage</param>
        /// <returns>Status result and remaining damage</returns>
        public DefenseResult TakeHit(int Damage, string damageType)
        {
            DefenseResult result = new DefenseResult();
            if (HP.Current <= 0)
            {
                result.Remainder = Damage;
                result.Messages.Add(string.Format("{0} is {1}!", this.Name, DownAdjective));
                return result;
            }

            int DR = 0;
            if (damageType == DamageType.Energy)
            {
                DR = this.DRVsEnergy;
            }
            else if(damageType == DamageType.Plasma)
            {
                DR = this.DRVsPlasma;
            }
            else if(damageType == DamageType.Explosive)
            {
                DR = this.DRVsExplosive;
            }
            else if(damageType==DamageType.Kinetic)
            {
                DR = this.DRVsKinetic;
            }

            if (Damage <= DR)
            {
                result.Remainder = 0;
                result.Messages.Add(string.Format("Bounces off {0} for No Damage!", this.Name));
            }
            else
            {
                int afterDR = Damage - DR;
                if (afterDR >= HP.Current)
                {
                    result.Remainder = afterDR - HP.Current;
                    result.Messages.Add(string.Format("Hits {0} for {1}{2}, {3} It!",
                        this.Name,
                        HP.Current,
                        (DR > 0 ? string.Format("(DR: {0})", DR) : string.Empty),
                        PenetrateVerb));
                    HP.Current = 0;
                }
                else
                {
                    result.Remainder = 0;
                    result.Messages.Add(string.Format("Hits {0} for {1}{2}", this.Name, afterDR, (DR > 0 ? string.Format("(DR: {0})", DR) : string.Empty)));
                    HP.Current -= afterDR;
                }
            }

            return result;
        }

        public override object Clone()
        {
            DefensePart copy = new DefensePart();
            copy.Name = (string)this.Name.Clone();
            copy.HP = (StatWithMax)this.HP.Clone();
            copy.IsDestroyed = this.IsDestroyed;
            copy.Mass = this.Mass;
            copy.Actions = (List<BaseAction>)this.Actions.Clone();
            copy.DRVsEnergy = this.DRVsEnergy;
            copy.DRVsPlasma = this.DRVsPlasma;
            copy.DRVsExplosive = this.DRVsExplosive;
            copy.DRVsKinetic = this.DRVsKinetic;
            copy.DownAdjective = (string)this.DownAdjective.Clone();
            copy.PenetrateVerb = (string)this.PenetrateVerb.Clone();
            return copy;
        }
        #endregion

        #region Constructors
        private DefensePart() : base() { }
        public DefensePart(PartData data, KeelClassification classification, List<BaseAction> actions)
            : base(data, classification, actions)
        {
            setBaseValuesFromDefenseType(data.DefenseType, classification);
        }
        #endregion

        void setBaseValuesFromDefenseType(string defenseType, KeelClassification classification)
        {
            if(defenseType == DefenseType.Shield)
            {
                this.DRVsEnergy = 3;
                this.DRVsPlasma = 2;
                this.DRVsExplosive = 1;
                this.DRVsKinetic = 0;
                this.DownAdjective = "Down";
                this.PenetrateVerb = "Penetrating";
            }
            else if(defenseType == DefenseType.Armor)
            {
                this.DRVsEnergy = 3;
                this.DRVsPlasma = 2;
                this.DRVsExplosive = 3;
                this.DRVsKinetic = 4;
                this.DownAdjective = "Destroyed";
                this.PenetrateVerb = "Shattering";
            }
            else if(defenseType == DefenseType.PointDefense)
            {
                this.DRVsEnergy = 0;
                this.DRVsPlasma = 0;
                this.DRVsExplosive = 4;
                this.DRVsKinetic = 4;
                this.DownAdjective = "Offline";
                this.PenetrateVerb = "Disabling";
            }
        }

    }
}
