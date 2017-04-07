using CodeFighter.Logic.Animations;
using CodeFighter.Logic.Parts;
using System.Collections.Generic;
using System;
using CodeFighter.Logic.Ships;
using CodeFighter.Logic.Utility;

namespace CodeFighter.Logic.Actions
{
    public class RepairPart : BaseAction
    {
        #region Public Methods
        public override string DoAction()
        {
            int repairAmount = Convert.ToInt32(ActionValues["RepairAmount"]);
            if (this.TargetPart.HP.Current < this.TargetPart.HP.Max)
            {
                if (this.TargetPart.HP.Current <= this.TargetPart.HP.Max - repairAmount)
                {
                    this.TargetPart.HP.Current += repairAmount;
                    return string.Format("{0} recovered {1}, current HP: {2}", this.TargetPart.Name, repairAmount, this.TargetPart.HP.Current);
                }
                else
                {
                    this.TargetPart.HP.Current = this.TargetPart.HP.Max;
                    return string.Format("{0}, current HP: {1}", this.TargetPart.Name, this.TargetPart.HP.Current);
                }
            }
            return string.Format("{0}, current HP: {1}", this.TargetPart.Name, this.TargetPart.HP.Current);
        }

        public override string ToString()
        {
            return string.Format("Repair this Part for {0} HPs", (Convert.ToInt32(ActionValues["RepairAmount"])).ToString());
        }

        public override object Clone()
        {
            RepairPart copy = new RepairPart();
            copy.ActionValues = (CloneableDictionary<string, object>)this.ActionValues.Clone();
            return copy;

        }
        #endregion

        #region Constructors
        private RepairPart() { }
        public RepairPart(BasePart targetPart, int amount)
        {
            this.TargetPart = targetPart;
            this.ActionValues = new CloneableDictionary<string, object>();
            this.ActionValues["RepairAmount"] = amount;
        }
        public RepairPart(BasePart targetPart, CloneableDictionary<string, object> actionValues)
        {
            this.TargetPart = targetPart;
            this.ActionValues = actionValues;
        }
        #endregion
    }
}
