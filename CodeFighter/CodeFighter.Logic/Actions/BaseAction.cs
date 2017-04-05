using CodeFighter.Logic.Animations;
using CodeFighter.Logic.Parts;
using CodeFighter.Logic.Ships;
using CodeFighter.Logic.Utility;
using System;
using System.Collections.Generic;

namespace CodeFighter.Logic.Actions
{
    public abstract class BaseAction : ICloneable
    {
        public BasePart TargetPart { get; set; }
        public Ship TargetShip { get; set; }
        public CloneableDictionary<string,object> ActionValues { get; set; }
        public abstract string DoAction();
        public abstract object Clone();
    }
}
