using CodeFighter.Logic.Actions;
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
        #endregion
        
        #region Constructors
        public ActionPart(string name, int maxHP, double mass, string actionDescription, List<BaseAction> actions)
            : base(name,maxHP,mass,actions)
        {
            Description = actionDescription;
            
        }
        
        #endregion

    }
}
