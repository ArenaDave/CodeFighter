using CodeFighter.Logic.Utility;
using System;

namespace CodeFighter.Logic.Scenarios
{
    public static class FeatureType
    {
        public static readonly string Asteroid = "asteroid";
    }

    public class ScenarioFeature : ICloneable
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public Point Position { get; set; }
        public int SpriteVertical { get; set; }
        public int SpriteHorizontal { get; set; }
        public bool IsBlocking { get; set; }

        public object Clone()
        {
            ScenarioFeature copy = new ScenarioFeature();
            copy.ID = this.ID;
            copy.Type = (string)this.Type.Clone();
            copy.Position = this.Position;
            copy.SpriteVertical = this.SpriteVertical;
            copy.SpriteHorizontal = this.SpriteHorizontal;
            copy.IsBlocking = this.IsBlocking;

            return copy;
        }
    }
}
