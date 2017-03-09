using CodeFighter.Logic.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFighter.Logic.Animations
{
    public class AnimationAddDetails : IAnimationDetails
    {
        public int id { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public bool isEnemy { get; set; }
        public int sizeCategory { get; set; }

        public AnimationAddDetails(int shipId, Point originPosition, bool isEnemy, int sizeCategory)
        {
            this.id = shipId;
            this.x = originPosition.X;
            this.y = originPosition.Y;
            this.isEnemy = isEnemy;
            this.sizeCategory = sizeCategory;
        }
    }

}
