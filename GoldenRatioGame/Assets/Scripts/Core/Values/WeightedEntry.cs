using System;
using UnityEngine;

namespace IM.Values
{
    [Serializable]
    public class WeightedEntry<T>
    {
        public T item;

        [Min(0f)] 
        public float weight = 1f; 
    }
}