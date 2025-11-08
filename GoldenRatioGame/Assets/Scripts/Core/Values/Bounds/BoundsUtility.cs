using System.Collections.Generic;
using UnityEngine;

namespace IM.Values
{
    public static class BoundsUtility
    {
        public static Bounds CreateBounds(IEnumerable<Vector3> points)
        {
            bool first = true;
            Vector3 min = Vector3.zero;
            Vector3 max = Vector3.zero;

            foreach (var point in points)
            {
                if (first)
                {
                    min = point;
                    max = point;
                    first = false;
                }
                else
                {
                    min = Vector3.Min(min, point);
                    max = Vector3.Max(max, point);
                }
            }

            if (first)
                return new Bounds(Vector3.zero, Vector3.zero);
            
            Vector3 center = (min + max) * 0.5f;
            Vector3 size = max - min;
            return new Bounds(center, size);
        }

        public static Bounds CreateBoundsNormalized(IEnumerable<Vector3> points)
        {
            bool first = true;
            Vector3 min = Vector3.zero;
            Vector3 max = Vector3.zero;

            foreach (var point in points)
            {
                if (first)
                {
                    min = point;
                    max = point;
                    first = false;
                }
                else
                {
                    min = Vector3.Min(min, point);
                    max = Vector3.Max(max, point);
                }
            }

            if (first)
                return new Bounds(Vector3.zero, Vector3.zero);
            
            Vector3 center = (min + max) * 0.5f;
            Vector3 size = max - min;
            
            return new Bounds(center, Vector3.one * Mathf.Max(size.x, size.y));
        }
    }
}