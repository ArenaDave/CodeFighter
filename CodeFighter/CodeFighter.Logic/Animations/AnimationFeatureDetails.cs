using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFighter.Logic.Utility;

namespace CodeFighter.Logic.Animations
{
    public class AnimationFeatureDetails : IAnimationDetails
    {
        public int id { get; set; }
        public bool isBlocking { get; set; }
        public Point position { get; set; }
        public string type { get; set; }

        public AnimationFeatureDetails(int id, Point position, string type, bool isBlocking)
        {
            this.id = id;
            this.position = position;
            this.type = type;
            this.isBlocking = isBlocking;
        }
    }
}
