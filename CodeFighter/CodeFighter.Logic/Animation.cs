using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace CodeFighter.Logic
{
    public static class ActionType
    {
        public const string Add = "add";
        public const string Kill = "kill";
        public const string Move = "move";
        public const string Shoot = "shoot";
        public const string Explosion = "explosion";
    }
    
    
    public class Animation
    {
        
        public string actionType { get; set; }
        
        public IAnimationDetails details { get; set; }
        public Animation()
        {

        }
        public Animation(string type, IAnimationDetails animationDetails)
        {
            this.actionType = type;
            this.details = animationDetails;
        }
    }

    public interface IAnimationDetails
    {

    }

    
    public class AnimationAdd : IAnimationDetails
    {
        public int id { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public bool isEnemy { get; set; }
        public int sizeCategory { get; set; }

        public AnimationAdd(int id, Point p, bool isEnemy, int sizeCategory)
        {
            this.id = id;
            this.x = p.X;
            this.y = p.Y;
            this.isEnemy = isEnemy;
            this.sizeCategory = sizeCategory;
        }
    }

    
    public class AnimationKill : IAnimationDetails
    {
        public int id { get; set; }

        public AnimationKill(int id)
        {
            this.id = id;
        }
    }

    
    public class AnimationMove : IAnimationDetails
    {
        public int id { get; set; }
        public int x { get; set; }
        public int y { get; set; }

        public AnimationMove(int id, Point p)
        {
            this.id = id;
            this.x = p.X;
            this.y = p.Y;
        }
    }

    
    public class AnimationShoot: IAnimationDetails
    {
        public List<AnimationShot> shots { get; set; }

        public AnimationShoot(List<AnimationShot> shots)
        {
            this.shots = shots;
        }
    }

    
    public class AnimationShot
    {
        public int oX { get; set; }
        public int oY { get; set; }
        public int tX { get; set; }
        public int tY { get; set; }
        public bool isEnemy { get; set; }
        public bool isHit { get; set; }
        public bool isCrit { get; set; }

        public AnimationShot(Point origin, Point target, bool isEnemy, bool isHit, bool isCrit)
        {
            oX = origin.X;
            oY = origin.Y;
            tX = target.X;
            tY = target.Y;
            this.isEnemy = isEnemy;
            this.isHit = isHit;
            this.isCrit = isCrit;
        }
    }

    
    public class AnimationExplosion : IAnimationDetails
    {
        public int x { get; set; }
        public int y { get; set; }

        public AnimationExplosion(Point p)
        {
            x = p.X;
            y = p.Y;
        }
    }

}
