using CodeFighter.Logic.Parts;
using CodeFighter.Logic.Utility;
using System;
using System.Collections.Generic;

namespace CodeFighter.Logic.Ships
{
    public class ShipHull : ICloneable
    {
        public int Id { get; internal set; }
        public Keel Size { get; internal set; }
        public StatWithMax Hitpoints { get; internal set; }
        public int MaxParts { get; internal set; }
        public List<PartCount> MaxPartsByType { get; set; }

        public string ClassName { get; internal set; }

        internal ShipHull()
        {

        }

        public ShipHull(string name, string hullSize)
        {
            ClassName = name;
            Size = Keel.ByDesignator(hullSize);
            MaxParts = Convert.ToInt32(Math.Round(Size.MaxAddedMass / Size.Classification.PartWeight));
            MaxPartsByType = new List<PartCount>();
            MaxPartsByType.Add(new PartCount(typeof(WeaponPart),"", Convert.ToInt32(Math.Round(MaxParts * (Size.Grade == "Light" ? 0.3 : 0.35)))));
            MaxPartsByType.Add(new PartCount(typeof(DefensePart),"", Convert.ToInt32(Math.Round(MaxParts * 0.2))));
            MaxPartsByType.Add(new PartCount(typeof(ActionPart),"", Convert.ToInt32(Math.Round(MaxParts * (Size.Grade == "Light" ? 0.1 : (Size.Grade == "Medium" ? 0.15 : 0.17))))));
            MaxPartsByType.Add(new PartCount(typeof(EnginePart),"", Convert.ToInt32(Math.Round(MaxParts * (Size.Grade == "Light" ? 0.6 : (Size.Grade == "Medium" ? 0.45 : 0.35))))));
            Hitpoints = new StatWithMax(Convert.ToInt32((Size.BaseMass + Size.MaxAddedMass) / 100));
        }

        public object Clone()
        {
            ShipHull copy = new ShipHull();
            copy.Size = (Keel)this.Size.Clone();
            copy.Hitpoints = (StatWithMax)this.Hitpoints.Clone();
            copy.ClassName = (string)this.ClassName.Clone();
            copy.MaxParts = this.MaxParts;
            copy.MaxPartsByType = this.MaxPartsByType.Clone();
            return copy;
        }
    }
}
