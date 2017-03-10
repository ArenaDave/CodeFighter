using CodeFighter.Logic.Actions;
using System.Collections.Generic;
using System;
using CodeFighter.Logic.Utility;
using CodeFighter.Logic.Ships;

namespace CodeFighter.Logic.Parts
{
    public class EnginePart : BasePart
    {
        public double Thrust { get; private set; }

        public override string ToString()
        {
            return string.Format("{0} ({1}kN/s)", this.Name, Thrust.ToString("0.0"));
        }

        public override object Clone()
        {
            EnginePart copy = new EnginePart();
            copy.Name = (string)this.Name.Clone();
            copy.HP = (StatWithMax)this.HP.Clone();
            copy.IsDestroyed = this.IsDestroyed;
            copy.Target = (Ship)this.Target.Clone();
            copy.Mass = this.Mass;
            copy.Actions = (List<BaseAction>)this.Actions.Clone();
            copy.Thrust = this.Thrust;
            return copy;
        }

        public EnginePart(string name, int maxHP, double mass, List<BaseAction> actions, double thrust)
            : base(name, maxHP, mass, actions)
        {
            this.Thrust = thrust;
        }

        public EnginePart() : base() { }
    }
}
