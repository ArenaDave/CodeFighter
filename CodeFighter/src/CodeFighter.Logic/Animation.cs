using System;
using System.Collections.Generic;
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
        public int ID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public bool isEnemy { get; set; }
        public int sizeCategory { get; set; }

        public AnimationAdd(int id, Point p, bool isEnemy, int sizeCategory)
        {
            this.ID = id;
            this.X = p.X;
            this.Y = p.Y;
            this.isEnemy = isEnemy;
            this.sizeCategory = sizeCategory;
        }
    }

    
    public class AnimationKill : IAnimationDetails
    {
        public int ID { get; set; }

        public AnimationKill(int id)
        {
            this.ID = id;
        }
    }

    
    public class AnimationMove : IAnimationDetails
    {
        public int ID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public AnimationMove(int id, Point p)
        {
            this.ID = id;
            this.X = p.X;
            this.Y = p.Y;
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
        public int X { get; set; }
        public int Y { get; set; }

        public AnimationExplosion(Point p)
        {
            X = p.X;
            Y = p.Y;
        }
    }

}
