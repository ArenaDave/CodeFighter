using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFighter.Logic.Utility
{
    public class CloneableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ICloneable
    {
        public object Clone()
        {
            CloneableDictionary<TKey, TValue> copy = new CloneableDictionary<TKey, TValue>(this.Count, this.Comparer);
            foreach (KeyValuePair<TKey, TValue> entry in this)
            {
                TKey key = entry.Key;
                Type keyType = entry.Key.GetType();
                if (keyType.IsClass)
                    if (entry.Key is ICloneable)
                        key = (TKey)((ICloneable)entry.Key).Clone();


                TValue val = entry.Value;
                Type valType = entry.Value.GetType();
                if (valType.IsClass)
                    if (entry.Value is ICloneable)
                        val = (TValue)((ICloneable)entry.Value).Clone();

                copy.Add(key, val);
            }

            return copy;
        }

        #region Constructors
        public CloneableDictionary(int capacity, IEqualityComparer<TKey> comp) : base(capacity, comp) { }
        public CloneableDictionary() : base() { }
        public CloneableDictionary(Dictionary<TKey,TValue> dictionary)
        {
            foreach (KeyValuePair<TKey, TValue> kvp in dictionary)
                this.Add(kvp.Key, kvp.Value);
        }
        #endregion
    }
}
