using CodeFighter.Logic.Utility;

namespace CodeFighter.Logic.Scenarios
{
    public static class FeatureType
    {
        public const string Asteroid = "asteroid";
    }

    public class ScenarioFeature
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public Point Position { get; set; }
        public int SpriteVertical { get; set; }
        public int SpriteHorizontal { get; set; }
        public bool IsBlocking { get; set; }
    }
}
