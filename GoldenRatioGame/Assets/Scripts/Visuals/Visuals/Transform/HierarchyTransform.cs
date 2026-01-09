using System;
using IM.Graphs;
using UnityEngine;

namespace IM.Visuals
{
    public class HierarchyTransform : HierarchyElement, IHierarchyTransform
    {
        private Vector3 _localPosition = Vector3.zero;
        private Vector3 _localScale = Vector3.one;
        private Quaternion _localRotation = Quaternion.identity;

        private bool _hasPendingWorldSnapshot;
        private TransformReadOnly _pendingWorldSnapshot;

        private bool _hasCachedParent;
        private Vector3 _cachedParentPosition;
        private Vector3 _cachedParentScale;
        private Quaternion _cachedParentRotation;

        public event Action<Vector3, Vector3> PositionChanged;
        public event Action<Quaternion, Quaternion> RotationChanged;
        public event Action<Vector3, Vector3> LossyScaleChanged;

        public event Action<Vector3, Vector3> LocalPositionChanged;
        public event Action<Vector3, Vector3> LocalScaleChanged;
        public event Action<Quaternion, Quaternion> LocalRotationChanged;

        public Vector3 LocalPosition
        {
            get => _localPosition;
            set
            {
                if (_localPosition == value) return;
                Vector3 old = _localPosition;
                ApplyLocalChangeAndNotify(() => _localPosition = value);
                LocalPositionChanged?.Invoke(old, value);
            }
        }

        public Vector3 LocalScale
        {
            get => _localScale;
            set
            {
                if (_localScale == value) return;
                Vector3 old = _localScale;
                ApplyLocalChangeAndNotify(() => _localScale = value);
                LocalScaleChanged?.Invoke(old, value);
            }
        }

        public Quaternion LocalRotation
        {
            get => _localRotation;
            set
            {
                if (_localRotation == value) return;
                Quaternion old = _localRotation;
                ApplyLocalChangeAndNotify(() => _localRotation = value);
                LocalRotationChanged?.Invoke(old, value);
            }
        }

        public Vector3 Position
        {
            get => GetWorldSnapshot().Position;
            set
            {
                TransformReadOnly oldWorld = GetWorldSnapshot();

                TransformOperations.SetLocalFromWorld(
                    this,
                    value,
                    oldWorld.LossyScale,
                    oldWorld.Rotation,
                    Parent as ITransformReadOnly
                );

                TransformReadOnly newWorld = GetWorldSnapshot();
                PropagateWorldToChildren();

                NotifyWorldChanges(oldWorld, newWorld);
            }
        }

        public Vector3 LossyScale => GetWorldSnapshot().LossyScale;

        public Quaternion Rotation
        {
            get => GetWorldSnapshot().Rotation;
            set
            {
                TransformReadOnly oldWorld = GetWorldSnapshot();

                if (Parent is ITransformReadOnly parent)
                    _localRotation = TransformOperations.LocalRotationFromWorld(value, parent);
                else
                    _localRotation = value;

                TransformReadOnly newWorld = GetWorldSnapshot();
                PropagateWorldToChildren();

                NotifyWorldChanges(oldWorld, newWorld);
            }
        }

        private void ApplyLocalChangeAndNotify(Action applyLocalChange)
        {
            TransformReadOnly before = GetWorldSnapshot();
            applyLocalChange();
            TransformReadOnly after = GetWorldSnapshot();

            PropagateWorldToChildren();
            NotifyWorldChanges(before, after);
        }

        private TransformReadOnly GetWorldSnapshot()
        {
            var result = TransformOperations.ComputeWorld(
                Parent as ITransformReadOnly,
                _localPosition,
                _localScale,
                _localRotation
            );

            return new TransformReadOnly(result.pos, result.scale, result.rot);
        }

        private void PropagateWorldToChildren()
        {
            TransformReadOnly world = GetWorldSnapshot();
            foreach (IHierarchyElement child in Children)
            {
                if (child is IHierarchyTransform childTransform)
                    childTransform.OnParentTransformChanged(world.Position, world.LossyScale, world.Rotation);
            }
        }

        private void NotifyWorldChanges(TransformReadOnly oldWorld, TransformReadOnly newWorld)
        {
            if (oldWorld.Position != newWorld.Position)
                PositionChanged?.Invoke(oldWorld.Position, newWorld.Position);

            if (oldWorld.LossyScale != newWorld.LossyScale)
                LossyScaleChanged?.Invoke(oldWorld.LossyScale, newWorld.LossyScale);

            if (oldWorld.Rotation != newWorld.Rotation)
                RotationChanged?.Invoke(oldWorld.Rotation, newWorld.Rotation);
        }

        public void OnParentTransformChanged(Vector3 parentPosition, Vector3 parentScale, Quaternion parentRotation)
        {
            if (!_hasCachedParent)
            {
                _cachedParentPosition = parentPosition;
                _cachedParentScale = parentScale;
                _cachedParentRotation = parentRotation;
                _hasCachedParent = true;
                PropagateWorldToChildren();
                return;
            }

            var oldWorldTuple = TransformOperations.ComputeWorld(
                new TransformReadOnly(_cachedParentPosition, _cachedParentScale, _cachedParentRotation),
                _localPosition, _localScale, _localRotation
            );
            var newWorldTuple = TransformOperations.ComputeWorld(
                new TransformReadOnly(parentPosition, parentScale, parentRotation),
                _localPosition, _localScale, _localRotation
            );

            var oldWorld = new TransformReadOnly(oldWorldTuple.pos, oldWorldTuple.scale, oldWorldTuple.rot);
            var newWorld = new TransformReadOnly(newWorldTuple.pos, newWorldTuple.scale, newWorldTuple.rot);

            _cachedParentPosition = parentPosition;
            _cachedParentScale = parentScale;
            _cachedParentRotation = parentRotation;

            NotifyWorldChanges(oldWorld, newWorld);
            PropagateWorldToChildren();
        }

        protected override void OnParentChanging(IHierarchyElement oldParent, IHierarchyElement newParent)
        {
            TransformReadOnly currentWorld = GetWorldSnapshot();
            _pendingWorldSnapshot = currentWorld;
            _hasPendingWorldSnapshot = true;
        }

        protected override void OnParentSet(IHierarchyElement parent)
        {
            if (_hasPendingWorldSnapshot)
            {
                TransformOperations.SetLocalFromWorld(
                    this,
                    _pendingWorldSnapshot.Position,
                    _pendingWorldSnapshot.LossyScale,
                    _pendingWorldSnapshot.Rotation,
                    parent as ITransformReadOnly
                );
                _hasPendingWorldSnapshot = false;
            }

            if (parent is ITransformReadOnly ip)
            {
                _cachedParentPosition = ip.Position;
                _cachedParentScale = ip.LossyScale;
                _cachedParentRotation = ip.Rotation;
                _hasCachedParent = true;
            }
            else
            {
                _hasCachedParent = false;
            }

            PropagateWorldToChildren();
        }

        protected override void OnChildAdded(IHierarchyElement child)
        {
            if (child is IHierarchyTransform t)
            {
                TransformReadOnly world = GetWorldSnapshot();
                t.OnParentTransformChanged(world.Position, world.LossyScale, world.Rotation);
            }
        }
    }
}