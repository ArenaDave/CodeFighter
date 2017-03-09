using CodeFighter.Logic.Animations;
using CodeFighter.Logic.Parts;
using CodeFighter.Logic.Ships;
using System.Collections.Generic;

namespace CodeFighter.Logic.Actions
{
    public abstract class BaseAction
    {
        public BasePart TargetPart { get; set; }
        public Ship TargetShip { get; set; }
        public Dictionary<string,object> ActionValues { get; set; }
        public abstract Animation DoAction();
    }
}
