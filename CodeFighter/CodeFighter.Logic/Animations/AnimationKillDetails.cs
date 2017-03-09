using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFighter.Logic.Animations
{
    public class AnimationKillDetails : IAnimationDetails
    {
        public int id { get; set; }

        public AnimationKillDetails(int shipId)
        {
            this.id = shipId;
        }
    }

}
