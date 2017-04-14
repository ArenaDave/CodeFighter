using CodeFighter.Logic.Actions;
using System.Collections.Generic;
using System;
using CodeFighter.Logic.Utility;
using CodeFighter.Logic.Ships;
using CodeFighter.Data;

namespace CodeFighter.Logic.Parts
{
    public class EnginePart : BasePart
    {
        #region Public Properties
        public double Thrust { get; private set; }
        #endregion

        #region Public Methods
        public override string ToString()
        {
            return string.Format("{0} ({1}kN/s){2}", this.Name, Thrust.ToString("0.0"), this.IsDestroyed ? " [DESTROYED!]" : "");
        }

        public override object Clone()
        {
            EnginePart copy = new EnginePart();
            copy.Name = (string)this.Name.Clone();
            copy.HP = (StatWithMax)this.HP.Clone();
            copy.IsDestroyed = this.IsDestroyed;
            copy.Mass = this.Mass;
            copy.Actions = (List<BaseAction>)this.Actions.Clone();
            copy.Thrust = this.Thrust;
            return copy;
        }
        #endregion

        #region Constructors
        
        public EnginePart() : base() { }

        public EnginePart(PartData data, KeelClassification classification, List<BaseAction> actions)
            : base(data, classification, actions)
        {
            this.Thrust = classification.BaseEngineThrust;
        }
        #endregion
    }
}
