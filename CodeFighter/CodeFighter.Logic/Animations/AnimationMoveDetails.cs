using CodeFighter.Logic.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFighter.Logic.Animations
{
    public class AnimationMoveDetails : IAnimationDetails
    {
        public int id { get; set; }
        public int x { get; set; }
        public int y { get; set; }

        public AnimationMoveDetails(int shipId, Point moveTo)
        {
            this.id = shipId;
            this.x = moveTo.X;
            this.y = moveTo.Y;
        }
    }
}
