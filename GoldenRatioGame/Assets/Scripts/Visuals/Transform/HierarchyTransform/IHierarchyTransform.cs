using IM.Graphs;
using UnityEngine;

namespace IM.Transforms
{
    public interface IHierarchyTransform : IHierarchyTransformReadOnly, ITransform, IHierarchyElement
    {
        public void AddChildKeepLocal(IHierarchyTransform child);
        void OnParentTransformChanged(Vector3 parentPosition, Vector3 parentScale, Quaternion parentRotation);
    }
}