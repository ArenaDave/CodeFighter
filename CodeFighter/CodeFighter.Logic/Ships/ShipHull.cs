using CodeFighter.Logic.Parts;
using CodeFighter.Logic.Utility;
using System;
using System.Collections.Generic;

namespace CodeFighter.Logic.Ships
{
    public class ShipHull : ICloneable
    {
        public Keel Size { get; private set; }
        public StatWithMax HullPoints { get;  private set; }
        public int MaxParts { get; private set; }
        public CloneableDictionary<Type,int> MaxPartsByType { get; private set; }

        public string ClassName { get; private set; }

        private ShipHull()
        {

        }

        public ShipHull(string name, string hullSize, int hp)
        {
            ClassName = name;
            HullPoints = new StatWithMax(hp);
            Size = Keel.ByDesignator(hullSize);
            MaxParts = Convert.ToInt32(Math.Round(Size.MaxAddedMass / Size.Classification.PartWeight));
            MaxPartsByType = new CloneableDictionary<Type, int>();
            MaxPartsByType[typeof(WeaponPart)] = Convert.ToInt32(Math.Round(MaxParts * (Size.Grade == "Light" ? 0.3 : 0.35)));
            MaxPartsByType[typeof(DefensePart)] = Convert.ToInt32(Math.Round(MaxParts * 0.2));
            MaxPartsByType[typeof(ActionPart)] = Convert.ToInt32(Math.Round(MaxParts * (Size.Grade == "Light" ? 0.1 : (Size.Grade == "Medium" ? 0.15 : 0.17))));
            MaxPartsByType[typeof(EnginePart)] = Convert.ToInt32(Math.Round(MaxParts * (Size.Grade == "Light" ? 0.6 : (Size.Grade == "Medium" ? 0.45 : 0.35))));
        }

        public object Clone()
        {
            ShipHull copy = new ShipHull();
            copy.Size = (Keel)this.Size.Clone();
            copy.HullPoints = (StatWithMax)this.HullPoints.Clone();
            copy.ClassName = (string)this.ClassName.Clone();
            copy.MaxParts = this.MaxParts;
            copy.MaxPartsByType = (CloneableDictionary<Type,int>)this.MaxPartsByType.Clone();
            return copy;
        }
    }
}
