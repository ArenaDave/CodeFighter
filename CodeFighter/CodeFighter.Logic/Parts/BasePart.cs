using CodeFighter.Data;
using CodeFighter.Logic.Actions;
using CodeFighter.Logic.Animations;
using CodeFighter.Logic.Ships;
using CodeFighter.Logic.Utility;
using System;
using System.Collections.Generic;

namespace CodeFighter.Logic.Parts
{
    public abstract class BasePart : ICloneable
    {
        public string Name { get; set; }
        public StatWithMax HP { get; set; }
        public bool IsDestroyed { get; set; }
        public Ship Target { get; set; }
        public double Mass { get; protected set; }
        public List<BaseAction> Actions { get; set; }

        #region Abstract Methods
        public abstract override string ToString();
        #endregion

        #region constructor
        public BasePart(string name, int maxHP, double mass, List<BaseAction> actions)
        {
            this.Name = name;
            this.HP = new StatWithMax(maxHP);
            this.Mass = mass;
            this.Actions = actions;
            this.IsDestroyed = false;
            this.Target = null;

        }

        public BasePart(PartData data, List<BaseAction> actions)
        {
            this.Name = data.Name;
            this.HP = new StatWithMax(data.MaxHP);
            this.Mass = data.Mass;
            this.Actions = actions;
            this.IsDestroyed = false;
            this.Target = null;
        }
        #endregion

        public virtual List<string> DoAction(Ship target)
        {
            if (this.Target == null)
            {
                this.Target = target;
            }

            List<string> results = new List<string>();
            foreach(BaseAction action in this.Actions)
            {
                action.TargetShip = this.Target;
                string r = action.DoAction();
                if(!string.IsNullOrEmpty(r))
                    results.Add(r);
            }

            return results;
        }

        public string Repair(int amount)
        {
            string result = string.Empty;

            if (this.IsDestroyed)
            {
                result = string.Format("Repaired {0}", this.Name);
                this.HP.Current = this.HP.Max;
                this.IsDestroyed = false;
            }

            if (this.HP.Current < this.HP.Max && amount > 0)
            {
                int previousHP = this.HP.Current;
                int amountRepaired = this.HP.Add(amount).Current - previousHP;
                result = string.Format("Repaired {0} for {1}", this.Name, amountRepaired);
            }

            return result;
        }

        protected BasePart() { }
        public abstract object Clone();
    }
}
