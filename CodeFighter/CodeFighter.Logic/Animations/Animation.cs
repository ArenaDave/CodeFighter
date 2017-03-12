using System;
using System.Collections.Generic;

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

    public class MessageEventArgs : EventArgs
    {
        public List<string> Messages { get; set; }
        public MessageEventArgs(List<string> messages)
        {
            this.Messages = messages;
        }
    }

    public delegate void MessageEvent(object sender, MessageEventArgs e);
    
    
    public class Animation
    {
        
        public string actionType { get; set; }
        
        public IAnimationDetails details { get; set; }

        public List<string> Messages { get; set; }

        public Animation()
        {

        }
        public Animation(string type, IAnimationDetails animationDetails, List<string> messages = null)
        {
            this.actionType = type;
            this.details = animationDetails;
            this.Messages = messages;
        }
    }
    
    

}
