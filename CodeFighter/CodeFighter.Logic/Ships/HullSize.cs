using System;

namespace CodeFighter.Logic.Ships
{
    public class HullSize : ICloneable
    {
        public double Mass { get; private set; }
        public string Image { get; private set; }
        public string Name { get; private set; }
        private HullSize(string name, double mass, string imageId)
        {
            Name = name;
            Mass = mass;
            Image = imageId;
        }

        #region Factory
        public static HullSize ByImg(string img)
        {
            switch (img)
            {
                case "DN":
                    return Dreadnought();
                case "BB":
                    return Battleship();
                case "BC":
                    return Battlecruiser();
                case "CV":
                    return Carrier();
                case "HC":
                    return HeavyCruiser();
                case "LC":
                    return LightCruiser();
                case "DD":
                    return Destroyer();
                case "FR":
                    return Frigate();
                case "CO":
                    return Corvette();
                default:
                    return Corvette();
            }

        }

        public static HullSize Dreadnought()
        {
            return new HullSize("Dreadnought", 5000.0, "DN");
        }
        public static HullSize Battleship()
        {
            return new HullSize("Battleship", 4000.0, "BB");
        }
        public static HullSize Battlecruiser()
        {
            return new HullSize("Battlecruiser", 3000.0, "BC");
        }
        public static HullSize Carrier()
        {
            return new HullSize("Carrier", 2500.0, "CV");
        }

        public static HullSize HeavyCruiser()
        {
            return new HullSize("HeavyCruiser", 2000.0, "HC");
        }

        public static HullSize LightCruiser()
        {
            return new HullSize("LightCruiser", 1000.0, "LC");
        }

        public static HullSize Destroyer()
        {
            return new HullSize("Destroyer", 800.0, "DD");
        }

        public static HullSize Frigate()
        {
            return new HullSize("Frigate", 500.0, "FR");
        }

        public static HullSize Corvette()
        {
            return new HullSize("Corvette", 350.0, "CO");
        }
        #endregion

        private HullSize() { }
        public object Clone()
        {
            HullSize copy = new HullSize();
            copy.Mass = this.Mass;
            copy.Image = (string)this.Image.Clone();
            copy.Name = (string)this.Name.Clone();
            return copy;
        }

        
    }
}
