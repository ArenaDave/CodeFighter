using System;
using System.Collections.Generic;

namespace CodeFighter.Logic.Animations
{
    public static class AnimationActionType
    {
        public static readonly string Add = "add";
        public static readonly string Kill = "kill";
        public static readonly string Move = "move";
        public static readonly string Shoot = "shoot";
        public static readonly string Explosion = "explosion";
        public static readonly string Message = "message";
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

        public List<string> messages { get; set; }

        public Animation()
        {

        }
        public Animation(string type, IAnimationDetails animationDetails, List<string> messages = null)
        {
            this.actionType = type;
            this.details = animationDetails;
            this.messages = messages;
        }
    }
    
    

}
