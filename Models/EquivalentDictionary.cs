using System;
using System.Collections.Generic;

namespace BlackjackGraphs.Models
{
    public class EquivalentDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public bool Equivalent(Dictionary<TKey, TValue> other)
        {
            var equivalent = true;
            foreach (var key in this.Keys)
            {
                if (other.ContainsKey(key))
                {
                    equivalent = equivalent && other[key].Equals(this[key]);
                }
                else
                {
                    return false;
                }
            }
            return equivalent;
        }
    }
}