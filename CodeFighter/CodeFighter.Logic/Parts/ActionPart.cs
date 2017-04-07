using CodeFighter.Data;
using CodeFighter.Logic.Actions;
using CodeFighter.Logic.Ships;
using CodeFighter.Logic.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFighter.Logic.Parts
{
    public class ActionPart : BasePart
    {
        #region Public Properties
        public string Description { get; private set; }
        #endregion

        #region Public Methods
        public override string ToString()
        {
            return string.Format("{0} ({1})", this.Name, Description);
        }

        public override object Clone()
        {
            ActionPart copy = new ActionPart();
            copy.Name = (string)this.Name.Clone();
            copy.HP = (StatWithMax)this.HP.Clone();
            copy.IsDestroyed = this.IsDestroyed;
            copy.Mass = this.Mass;
            copy.Actions = (List<BaseAction>)this.Actions.Clone();
            copy.Description = (string)this.Description.Clone();
            return copy;
        }
        #endregion

        #region Constructors
        public ActionPart(string name, int maxHP, double mass, string actionDescription, List<BaseAction> actions)
            : base(name,maxHP,mass,actions)
        {
            Description = actionDescription;
            
        }

        private ActionPart():base() { }

        public ActionPart(PartData data, List<BaseAction> actions)
            :base(data,actions)
        {
            Description = data.Description;
        }
        #endregion

        


    }
}
