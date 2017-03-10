using System;

namespace CodeFighter.Logic.Utility
{

    public class StatWithMax : ICloneable
    {
        int max = 0;
        int current = 0;

        public int Max { get { return max; } set { max = value; current = value; } }
        public int Current { get { return current; } set { current = value; } }

        public override string ToString()
        {
            return string.Format("{0}/{1}", Current.ToString("D" + Max.ToString().Length.ToString()), Max.ToString());
        }

        public StatWithMax Add(int amount)
        {
            StatWithMax result = this;
            if (amount == int.MaxValue)
            {
                Current = Max;
            }
            else
            {
                Current = Math.Min(Max, Current + amount);
            }
            return result;
        }

        public static StatWithMax operator +(StatWithMax swm, int addition)
        {
            return swm.Add(addition);
        }

        public StatWithMax Reduce(int amount)
        {
            StatWithMax result = this;
            Current = Math.Max(0, Current - amount);
            return result;
        }
        

        public static StatWithMax operator -(StatWithMax swm, int subtraction)
        {
            return swm.Reduce(subtraction);
        }

        public StatWithMax(int Max)
        {
            current = Max;
            max = Max;
        }

        private StatWithMax() { }

        public object Clone()
        {
            StatWithMax copy = new StatWithMax();
            copy.max = this.max;
            copy.current = this.current;
            return copy;
        }
    }

}
