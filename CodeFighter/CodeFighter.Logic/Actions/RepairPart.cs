using CodeFighter.Logic.Animations;
using CodeFighter.Logic.Parts;
using System.Collections.Generic;

namespace CodeFighter.Logic.Actions
{
    public class RepairPart : BaseAction
    {
        #region Public Methods
        public override Animation DoAction()
        {
            int repairAmount = (int)ActionValues["RepairAmount"];
            if (this.TargetPart.HP.Current < this.TargetPart.HP.Max)
            {
                if (this.TargetPart.HP.Current <= this.TargetPart.HP.Max - repairAmount)
                {
                    this.TargetPart.HP.Current += repairAmount;
                    return new Animation(AnimationActionType.Message, null, string.Format("{0} recovered {1}, current HP: {2}", this.TargetPart.Name, repairAmount, this.TargetPart.HP.Current));
                }
                else
                {
                    this.TargetPart.HP.Current = this.TargetPart.HP.Max;
                    return new Animation(AnimationActionType.Message, null, string.Format("{0}, current HP: {0}", this.TargetPart.Name, this.TargetPart.HP.Current));
                }
            }
            return new Animation(AnimationActionType.Message, null, string.Format("{0}, current HP: {0}", this.TargetPart.Name, this.TargetPart.HP.Current));
        }
        
        public override string ToString()
        {
            return string.Format("Repair this Part for {0} HPs", ((int)ActionValues["RepairAmount"]).ToString());
        }
        #endregion

        #region Constructors
        public RepairPart(BasePart targetPart, int amount)
        {
            this.TargetPart = targetPart;
            this.ActionValues["RepairAmount"] = amount;
        }
        public RepairPart(BasePart targetPart, Dictionary<string, object> actionValues)
        {
            this.TargetPart = targetPart;
            this.ActionValues = actionValues;
        }
        #endregion
    }
}
