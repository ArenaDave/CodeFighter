using CodeFighter.Logic.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFighter.Logic.Animations
{
    public class AnimationExplosionDetails : IAnimationDetails
    {
        public int x { get; set; }
        public int y { get; set; }

        public AnimationExplosionDetails(Point explosionPosition)
        {
            x = explosionPosition.X;
            y = explosionPosition.Y;
        }
    }
}
