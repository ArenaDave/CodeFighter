using System;

namespace CodeFighter.Logic.Ships
{
    public struct KeelSize
    {
        public string Name { get; internal set; }
        public double Factor { get; internal set; }
        public double PartWeight { get; internal set; }
    }

    public static class KeelSizeFactory
    {
        public static readonly KeelSize Escort = new KeelSize() { Name = "Escort", Factor = 4, PartWeight=100 }; // best thrust per engine: 900
        public static readonly KeelSize Cruiser = new KeelSize() { Name = "Cruiser", Factor = 5, PartWeight=200 }; // best thrust per engine: 1500
        public static readonly KeelSize Capital = new KeelSize() { Name = "Capital", Factor = 5, PartWeight=300 }; // best thrust per engine: 2350
    }

    public class Keel : ICloneable
    {
        public KeelSize Classification { get; private set; }
        public string Grade { get; private set; }
        public double BaseMass { get; private set; }
        public double MaxAddedMass { get; private set; }
        public string Designator { get; private set; }
        public string Name { get; private set; }

        private Keel(string name, string designator, double baseMass, double maxAddedMass)
        {
            Name = name;
            BaseMass = baseMass;
            MaxAddedMass = maxAddedMass;
            Designator = designator;
            switch (designator)
            {
                case "DN":
                    Classification = KeelSizeFactory.Capital;
                    Grade = "Heavy";
                    break;
                case "BB":
                    Classification = KeelSizeFactory.Capital;
                    Grade = "Medium";
                    break;
                case "CV":
                    Classification = KeelSizeFactory.Capital;
                    Grade = "Light";
                    break;
                case "BC":
                    Classification = KeelSizeFactory.Cruiser;
                    Grade = "Heavy";
                    break;
                case "HC":
                    Classification = KeelSizeFactory.Cruiser;
                    Grade = "Medium";
                    break;
                case "LC":
                    Classification = KeelSizeFactory.Cruiser;
                    Grade = "Light";
                    break;
                case "DD":
                    Classification = KeelSizeFactory.Escort;
                    Grade = "Heavy";
                    break;
                case "FR":
                    Classification = KeelSizeFactory.Escort;
                    Grade = "Medium";
                    break;
                case "CO":
                    Classification = KeelSizeFactory.Escort;
                    Grade = "Light";
                    break;
            }
        }

        #region Factory
        public static Keel ByDesignator(string designator)
        {
            switch (designator)
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

        public static Keel Dreadnought()
        {
            return new Keel("Dreadnought", "DN",22000.0,7500.0);
        }
        public static Keel Battleship()
        {
            return new Keel("Battleship", "BB", 16000.0,6000.0);
        }
        public static Keel Battlecruiser()
        {
            return new Keel("Battlecruiser", "BC", 14000.0,5000.0);
        }
        public static Keel Carrier()
        {
            return new Keel("Carrier", "CV", 12000.0,4500.0);
        }
        public static Keel HeavyCruiser()
        {
            return new Keel("HeavyCruiser", "HC", 10000.0,4000.0);
        }
        public static Keel LightCruiser()
        {
            return new Keel("LightCruiser", "LC", 7500.0,3000.0);
        }
        public static Keel Destroyer()
        {
            return new Keel("Destroyer", "DD",6500.0,2500.0);
        }
        public static Keel Frigate()
        {
            return new Keel("Frigate", "FR",5000.0,2000.0);
        }
        public static Keel Corvette()
        {
            return new Keel("Corvette", "CO", 4000.0,1500.0);
        }
        #endregion

        private Keel() { }
        public object Clone()
        {
            Keel copy = new Keel();
            copy.BaseMass = this.BaseMass;
            copy.Designator = (string)this.Designator.Clone();
            copy.Name = (string)this.Name.Clone();
            copy.MaxAddedMass = this.MaxAddedMass;
            copy.Classification = this.Classification;
            return copy;
        }


    }
}
