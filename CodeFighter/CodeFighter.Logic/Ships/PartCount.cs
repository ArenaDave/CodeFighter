using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFighter.Logic.Ships
{
    public class PartCount : ICloneable
    {
        public Type PartType { get; internal set; }
        public string ActionMechanism { get; internal set; }
        public int CountOfParts { get; internal set; }

        public override string ToString()
        {
            return string.Format("{0}{1} ({2})",
                PartType.Name,
                (ActionMechanism != string.Empty ? string.Format(" ({0})", ActionMechanism) : string.Empty),
                CountOfParts.ToString());
        }

        public object Clone()
        {
            PartCount copy = new PartCount(PartType,ActionMechanism,CountOfParts);
            return copy;
        }

        internal PartCount() { }

        public PartCount(Type partType, string actionMechanism, int countOfParts)
        {
            this.PartType = partType;
            this.ActionMechanism = actionMechanism;
            this.CountOfParts = countOfParts;
        }
    }
}
