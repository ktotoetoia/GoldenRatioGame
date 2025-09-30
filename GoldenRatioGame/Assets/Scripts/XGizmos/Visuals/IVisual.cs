using UnityEngine;

namespace IM.ModuleGraphGizmosDebug
{
    public interface IVisual
    {
        Vector3 Position { get; }
        bool ContainsPoint(Vector3 point);
        void MoveTo(Vector3 point);
        void DrawGizmo();
    }
}