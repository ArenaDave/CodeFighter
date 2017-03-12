using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFighter.Logic.Utility
{
    public class AttackResult
    {
        public List<string> Messages = new List<string>();
        public bool IsHit;
        public bool IsCrit;
    }
}
