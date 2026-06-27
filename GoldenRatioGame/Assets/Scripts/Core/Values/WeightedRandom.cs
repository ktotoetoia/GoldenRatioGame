using System;
using System.Collections.Generic;
using UnityEngine;

namespace IM.Values
{
    [Serializable]
    public class WeightedRandom<T>
    {
        [SerializeField] private List<WeightedEntry<T>> entries = new();
        private System.Random random = new ();

        public System.Random Random { get => random; set => random = value ?? new System.Random(); }

        public IEnumerable<WeightedEntry<T>> Entries => entries;

        public T Next()
        {
            if (entries == null || entries.Count == 0)
            {
                Debug.LogWarning("WeightedRandom collection is empty. Returning default value.");
                return default;
            }

            double totalWeight = 0;
            foreach (var entry in entries)
            {
                totalWeight += entry.weight;
            }

            if (totalWeight <= 0)
            {
                Debug.LogWarning("Total weight of WeightedRandom collection is 0. Returning default value.");
                return default;
            }

            double roll = random.NextDouble() * totalWeight;

            foreach (var entry in entries)
            {
                if (entry.weight <= 0) continue;

                roll -= entry.weight;

                if (roll <= 0) 
                {
                    return entry.item;
                }
            }

            return entries[^1].item; 
        }

        public void Add(T item, float weight)
        {
            entries.Add(new WeightedEntry<T> { item = item, weight = Mathf.Max(0, weight) });
        }
    }
}