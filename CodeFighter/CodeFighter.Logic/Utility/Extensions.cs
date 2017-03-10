using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFighter.Logic.Utility
{
    public static class Extensions
    {
        public static List<T> Clone<T>(this List<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }

        public static void Shuffle<T>(this List<T> list)
        {
            using (RNG rng = new RNG())
            {
                int n = list.Count;
                while (n > 1)
                {
                    n--;
                    int k = rng.d(n + 1);
                    T value = list[k];
                    list[k] = list[n];
                    list[n] = value;
                }
            }
        }
    }
}
