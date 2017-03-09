namespace CodeFighter.Logic.Animations
{
    public static class AnimationActionType
    {
        public const string Add = "add";
        public const string Kill = "kill";
        public const string Move = "move";
        public const string Shoot = "shoot";
        public const string Explosion = "explosion";
        public const string Message = "message";
    }
    
    
    public class Animation
    {
        
        public string actionType { get; set; }
        
        public IAnimationDetails details { get; set; }

        public string Message { get; set; }

        public Animation()
        {

        }
        public Animation(string type, IAnimationDetails animationDetails, string message = "")
        {
            this.actionType = type;
            this.details = animationDetails;
            this.Message = message;
        }
    }
    
    

}
