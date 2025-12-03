using System.Collections.Generic;
using IM.Graphs;
using UnityEngine;

namespace IM.Visuals
{
    public class TransformComponent : MonoBehaviour, ITransform
    {
        private ITransform _transform;
        
        public Vector3 Position
        {
            get => _transform.Position;
            set => _transform.Position = value;
        }

        public Vector3 LocalPosition
        {
            get => _transform.LocalPosition;
            set => _transform.LocalPosition = value;
        }

        public Vector3 LossyScale => _transform.LossyScale;

        public Vector3 LocalScale
        {
            get => _transform.LocalScale;
            set => _transform.LocalScale = value;
        }

        public Quaternion Rotation
        {
            get => _transform.Rotation;
            set => _transform.Rotation = value;
        }

        public Quaternion LocalRotation
        {
            get => _transform.LocalRotation;
            set => _transform.LocalRotation = value;
        }

        public void OnParentTransformChanged(Vector3 parentPosition, Vector3 parentScale, Quaternion parentRotation)
        {
            _transform.OnParentTransformChanged(parentPosition, parentScale, parentRotation);
        }

        public IEnumerable<IEdge> Edges => _transform.Edges;

        public IHierarchyElement Parent => _transform.Parent;

        public IReadOnlyList<IHierarchyElement> Children => _transform.Children;

        public bool ContainsChild(IHierarchyElement child)
        {
            return _transform.ContainsChild(child);
        }

        public void AddChild(IHierarchyElement child)
        {
            _transform.AddChild(child);
        }

        public bool RemoveChild(IHierarchyElement child)
        {
            return _transform.RemoveChild(child);
        }

        public void SetParent(IHierarchyElement newParent)
        {
            _transform.SetParent(newParent);
        }

        public void AddChildInternal(IHierarchyElement child)
        {
            _transform.AddChildInternal(child);
        }

        public bool RemoveChildInternal(IHierarchyElement child)
        {
            return _transform.RemoveChildInternal(child);
        }

        public void SetParentInternal(IHierarchyElement parent)
        {
            _transform.SetParentInternal(parent);
        }
    }
}