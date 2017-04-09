using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFighter.Logic.Animations
{
    public class AnimationShotDetails
    {
        public int origin { get; set; }
        public int target { get; set; }
        public bool isHit { get; set; }
        public bool isCrit { get; set; }
        public string firingType { get; set; }

        public AnimationShotDetails(int originShip, int targetShip, bool isHit, bool isCrit, string firingType)
        {
            origin = originShip;
            target = targetShip;
            this.isHit = isHit;
            this.isCrit = isCrit;
            this.firingType = firingType;
        }
    }
}
