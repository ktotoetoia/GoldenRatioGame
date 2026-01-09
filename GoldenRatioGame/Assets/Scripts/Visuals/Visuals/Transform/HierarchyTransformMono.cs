using System.Collections.Generic;
using IM.Graphs;
using UnityEngine;

namespace IM.Visuals
{
    public class HierarchyTransformMono : MonoBehaviour, IHierarchyTransform
    {
        private readonly HierarchyTransform _hierarchyTransform = new();

        private void Awake()
        {
            _hierarchyTransform.PositionChanged += (_, newValue) => transform.position = newValue;
            _hierarchyTransform.LossyScaleChanged += (_, newValue) => transform.localScale = newValue;
            _hierarchyTransform.RotationChanged += (_, newValue) => transform.localRotation = newValue;
        }
        
        public Vector3 Position
        {
            get => _hierarchyTransform.Position;
            set => _hierarchyTransform.Position = value;
        }

        public Vector3 LocalPosition
        {
            get => _hierarchyTransform.LocalPosition;
            set => _hierarchyTransform.LocalPosition = value;
        }

        public Vector3 LossyScale => ((IHaveScale)_hierarchyTransform).LossyScale;

        public Vector3 LocalScale
        {
            get => _hierarchyTransform.LocalScale;
            set => _hierarchyTransform.LocalScale = value;
        }

        public Quaternion Rotation
        {
            get => _hierarchyTransform.Rotation;
            set => _hierarchyTransform.Rotation = value;
        }

        public Quaternion LocalRotation
        {
            get => _hierarchyTransform.LocalRotation;
            set => _hierarchyTransform.LocalRotation = value;
        }

        public IEnumerable<IEdge> Edges => _hierarchyTransform.Edges;

        IHierarchyElementReadOnly IHierarchyElementReadOnly.Parent => ((IHierarchyElementReadOnly)_hierarchyTransform).Parent;

        public IReadOnlyList<IHierarchyElement> Children => _hierarchyTransform.Children;

        public void AddChild(IHierarchyElement child)
        {
            _hierarchyTransform.AddChild(child);
        }

        public bool RemoveChild(IHierarchyElement child)
        {
            return _hierarchyTransform.RemoveChild(child);
        }

        public void SetParent(IHierarchyElement newParent)
        {
            _hierarchyTransform.SetParent(newParent);
        }

        public void AddChildInternal(IHierarchyElement child)
        {
            _hierarchyTransform.AddChildInternal(child);
        }

        public bool RemoveChildInternal(IHierarchyElement child)
        {
            return _hierarchyTransform.RemoveChildInternal(child);
        }

        public void SetParentInternal(IHierarchyElement parent)
        {
            _hierarchyTransform.SetParentInternal(parent);
        }

        public void OnParentTransformChanged(Vector3 parentPosition, Vector3 parentScale, Quaternion parentRotation)
        {
            _hierarchyTransform.OnParentTransformChanged(parentPosition, parentScale, parentRotation);
        }

        public IHierarchyElement Parent => _hierarchyTransform.Parent;

        IReadOnlyList<IHierarchyElementReadOnly> IHierarchyElementReadOnly.Children => ((IHierarchyElementReadOnly)_hierarchyTransform).Children;

        public bool ContainsChild(IHierarchyElementReadOnly child)
        {
            return _hierarchyTransform.ContainsChild(child);
        }
    }
}