using CodeFighter.Logic.Actions;
using System.Collections.Generic;

namespace CodeFighter.Logic.Parts
{
    public class EnginePart : BasePart
    {
        public double Thrust { get; private set; }

        public override string ToString()
        {
            return string.Format("{0} ({1}kN/s)", this.Name, Thrust.ToString("0.0"));
        }

        public EnginePart(string name, int maxHP, double mass, List<BaseAction> actions, double thrust)
            : base(name, maxHP, mass, actions)
        {
            this.Thrust = thrust;
        }


    }
}
