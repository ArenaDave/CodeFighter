using CodeFighter.Logic.Animations;
using CodeFighter.Logic.Parts;
using CodeFighter.Logic.Ships;
using CodeFighter.Logic.Utility;
using System.Collections.Generic;
using System.Linq;

namespace CodeFighter.Logic.Actions
{
    public class RepairShip : BaseAction
    {
        #region Public Methods
        public override Animation DoAction()
        {
            int repairAmount = (int)ActionValues["RepairAmount"];
            List<string> repaired = new List<string>();
            string result = string.Empty;
            using (RNG rand = new RNG())
            {
                foreach (BasePart part in TargetShip.Parts.Where(f => f.IsDestroyed))
                    if (rand.d100() > 50)
                    {
                        result = part.Repair(repairAmount);
                        repaired.Add(part.Name + (!string.IsNullOrEmpty(result) ? string.Format(" ({0} HPs)", result) : string.Empty));
                    }
            }

            int oldHP = TargetShip.HP.Current;
            int newHP = TargetShip.HP.Add(repairAmount).Current;
            result = string.Format("Repaired {0} for {1} HPs", TargetShip.Name, (newHP-oldHP));
            if (repaired.Count > 0)
                result = result + string.Format(", and Repaired {0}", string.Join(", ", repaired.ToArray()));

            return new Animation(AnimationActionType.Message,null, new List<string>() { result });
        }
        
        public override string ToString()
        {
            return string.Format("Repair target Ship  for {0} HPs, 50% chance to repair Parts", ((int)ActionValues["RepairAmount"]).ToString());
        }

        public override object Clone()
        {
            RepairShip copy = new RepairShip();
            //copy.TargetPart = (BasePart)this.TargetPart?.Clone();
            //copy.TargetShip = (Ship)this.TargetShip?.Clone();
            copy.ActionValues = (CloneableDictionary<string, object>)this.ActionValues.Clone();
            return copy;

        }
        #endregion

        #region Constructors
        private RepairShip() : base() { }
        public RepairShip(Ship targetShip, int amount)
        {
            this.TargetShip = targetShip;
            this.ActionValues = new CloneableDictionary<string, object>();
            this.ActionValues["RepairAmount"] = amount;
        }
        /// <summary>
        /// Instantiates RepairTargetEidos with specified ActionValues.
        /// </summary>
        /// <param name="ActionValues">ActionValues[0] will be cast to int as the amount to repair</param>
        public RepairShip(Ship targetShip, CloneableDictionary<string,object> actionValues)
        {
            this.TargetShip = targetShip;
            this.ActionValues = actionValues;
        }

        #endregion


    }
}
