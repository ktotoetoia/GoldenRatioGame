using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GRP.GoldenRatio
{
    public class PointsBasedGoldenRatio : IGoldenRatio
    {
        private readonly List<Vector2> _points;
        public IReadOnlyList<Vector2> Points => _points;
        
        public PointsBasedGoldenRatio(float arcStep, float maxS, float rotationDegrees = 0f)
        {
            if (arcStep <= 0)
            {
                throw new ArgumentException();
            }

            _points = new List<Vector2>();

            float a = 1f;
            float b = 0.306349f;

            float radians = rotationDegrees * Mathf.Deg2Rad;

            for (float s = 0; s < maxS; s += arcStep)
            {
                float theta = 1f / b * Mathf.Log((b * s) / (a * Mathf.Sqrt(1 + b * b)) + 1f);
                float r = a * Mathf.Exp(b * theta);
                float x = r * Mathf.Cos(theta);
                float y = r * Mathf.Sin(theta);
                float rotX = x * Mathf.Cos(radians) - y * Mathf.Sin(radians);
                float rotY = x * Mathf.Sin(radians) + y * Mathf.Cos(radians);

                _points.Add(new Vector2(rotX, rotY));
            }
        }

        public float DistanceToSpiral(Vector2 position)
        {
            return Points.Min(x => Vector2.Distance(position, x));
        }
    }
}