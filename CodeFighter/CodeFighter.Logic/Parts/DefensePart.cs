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
        public string DownAdjective { get; private set; }
        public string PenetrateVerb { get; private set; }
        #endregion

        #region Private Methods
        #endregion

        #region Public Methods
        public override string ToString()
        {
            return string.Format("{0} (DR:{1}) (HP:{2}/{3}){4}", this.Name, DR.ToString(), HP.Current.ToString(), HP.Max.ToString(), this.IsDestroyed ? " [DESTROYED!]" : "");
        }
        /// <summary>
        /// Handles incoming damage
        /// </summary>
        /// <param name="Damage">Amount of incoming damage</param>
        /// <returns>Status result and remaining damage</returns>
        public DefenseResult TakeHit(int Damage)
        {
            DefenseResult result = new DefenseResult();
            if (HP.Current <= 0)
            {
                result.Remainder = Damage;
                result.Messages.Add(string.Format("{0} is {1}!", this.Name, DownAdjective));
                return result;
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
            copy.DR = this.DR;
            copy.DownAdjective = (string)this.DownAdjective.Clone();
            copy.PenetrateVerb = (string)this.PenetrateVerb.Clone();
            return copy;
        }
        #endregion

        #region Constructors
        public DefensePart(string name, int maxHP, double mass, int damageReduction, string downAdjective, string penetrateVerb, List<BaseAction> actions)
            : base(name, maxHP, mass, actions)
        {
            this.DR = damageReduction;
            this.DownAdjective = downAdjective;
            this.PenetrateVerb = penetrateVerb;
        }
        private DefensePart() : base() { }
        public DefensePart(PartData data, List<BaseAction> actions)
            : base(data, actions)
        {
            this.DR = data.DR;
            this.DownAdjective = data.DownAdjective;
            this.PenetrateVerb = data.PenetrateVerb;
        }
        #endregion

    }
}
