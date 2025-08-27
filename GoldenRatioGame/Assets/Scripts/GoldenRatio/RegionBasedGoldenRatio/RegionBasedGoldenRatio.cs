using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IM.GoldenRatio
{
    public class RegionBasedGoldenRatio : IGoldenRatio
    {
        public IReadOnlyList<RatioRegion> Regions { get; }

        public RegionBasedGoldenRatio(IReadOnlyList<RatioRegion> regions)
        {
            Regions = regions;
        }

        public float DistanceToSpiral(Vector2 position)
        {
            float distance = Mathf.Infinity;

            foreach (RatioRegion region in Regions)
            {
                if (!region.TryGetDistance(out float newDistance, position))
                {
                    distance = Mathf.Min(distance, newDistance);
                }
            }
            
            return distance;
        }
    }
}