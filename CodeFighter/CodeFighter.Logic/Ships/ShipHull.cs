using CodeFighter.Logic.Utility;

namespace CodeFighter.Logic.Ships
{
    public class ShipHull
    {
        public HullSize Size { get; set; }
        public StatWithMax HullPoints { get; set; }
        public string ClassName { get; set; }
        public ShipHull(string name, string hullSize, int hp)
        {
            ClassName = name;
            HullPoints = new StatWithMax(hp);
            Size = HullSize.ByImg(hullSize);
        }

    }
}
