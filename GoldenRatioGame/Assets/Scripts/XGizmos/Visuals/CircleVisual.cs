using UnityEngine;

namespace IM.ModuleGraphGizmosDebug
{
    public class CircleVisual : IVisual
    {
        public Vector3 Position { get; private set; }
        public float Radius { get; set; }
        
        public CircleVisual(Vector3 position, float radius)
        {
            Position = position;
            Radius = radius;
        }
        
        public bool ContainsPoint(Vector3 point)
        {
            return Vector3.Distance(Position, point) < Radius;
        }

        public void MoveTo(Vector3 point)
        {
            Position = point;
        }

        public void DrawGizmos()
        {
            Gizmos.DrawSphere(Position, Radius);
        }
    }
}