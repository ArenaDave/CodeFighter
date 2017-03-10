using CodeFighter.Logic.Utility;
using System;

namespace CodeFighter.Logic.Ships
{
    public class ShipHull : ICloneable
    {
        public HullSize Size { get; set; }
        public StatWithMax HullPoints { get; set; }
        public string ClassName { get; set; }

        private ShipHull()
        {

        }

        public ShipHull(string name, string hullSize, int hp)
        {
            ClassName = name;
            HullPoints = new StatWithMax(hp);
            Size = HullSize.ByImg(hullSize);
        }

        public object Clone()
        {
            ShipHull copy = new ShipHull();
            copy.Size = (HullSize)this.Size.Clone();
            copy.HullPoints = (StatWithMax)this.HullPoints.Clone();
            copy.ClassName = (string)this.ClassName.Clone();
            return copy;
        }
    }
}
