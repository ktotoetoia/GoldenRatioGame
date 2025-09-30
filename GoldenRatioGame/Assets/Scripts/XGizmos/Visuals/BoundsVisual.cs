using UnityEngine;

namespace IM.ModuleGraphGizmosDebug
{
    public class BoundsVisual : IVisual
    {
        public Bounds Bounds { get; private set; }
        public Vector3 Position => Bounds.center;

        public BoundsVisual(Vector3 position, Vector3 size) : this(new Bounds(position, size))
        {
            
        }
        
        public BoundsVisual(Bounds bounds)
        {
            Bounds = bounds;
        }
        
        public bool ContainsPoint(Vector3 point)
        {
            return Bounds.Contains(point);
        }

        public void MoveTo(Vector3 point)
        {
            Bounds = new Bounds(point, Bounds.size);
        }

        public void DrawGizmo()
        {
            Gizmos.DrawCube(Bounds.center, Bounds.size);
        }
    }
}