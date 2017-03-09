
using CodeFighter.Logic.Utility;

namespace CodeFighter.Logic.Scenarios
{
    public class ScenarioShip
    {
        public int ID { get; set; }
        public Point StartingPosition { get; set; }
        public bool IsEnemy { get; set; }
        public int SizeCategory { get; set; }
    }
}
