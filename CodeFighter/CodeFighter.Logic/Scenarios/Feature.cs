using CodeFighter.Logic.Utility;
using System;

namespace CodeFighter.Logic.Scenarios
{
    public static class FeatureType
    {
        public static readonly string Asteroid = "asteroid";
    }

    public class Feature : ICloneable
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public Point Position { get; set; }
        public bool IsBlocking { get; set; }

        public object Clone()
        {
            Feature copy = new Feature();
            copy.ID = this.ID;
            copy.Type = (string)this.Type.Clone();
            copy.Position = this.Position;
            copy.IsBlocking = this.IsBlocking;

            return copy;
        }
    }
}
