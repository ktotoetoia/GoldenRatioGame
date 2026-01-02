using System;
using IM.Graphs;
using UnityEngine;

namespace IM.Visuals
{
    public class HierarchyTransform : HierarchyElement, IHierarchyTransform
    {
        private readonly ParentlessTransform _core;

        private bool _hasPendingOldWorld;
        private Vector3 _pendingOldWorldPos;
        private Vector3 _pendingOldWorldScale;
        private Quaternion _pendingOldWorldRot;

        private Vector3 _lastParentPos;
        private Vector3 _lastParentScale;
        private Quaternion _lastParentRot;
        private bool _hasLastParent;

        public event Action<Vector3, Vector3> PositionChanged;
        public event Action<Quaternion, Quaternion> RotationChanged;
        public event Action<Vector3, Vector3> LossyScaleChanged;

        public event Action<Vector3, Vector3> LocalPositionChanged
        {
            add => _core.LocalPositionChanged += value;
            remove => _core.LocalPositionChanged -= value;
        }

        public event Action<Vector3, Vector3> LocalScaleChanged
        {
            add => _core.LocalScaleChanged += value;
            remove => _core.LocalScaleChanged -= value;
        }

        public event Action<Quaternion, Quaternion> LocalRotationChanged
        {
            add => _core.LocalRotationChanged += value;
            remove => _core.LocalRotationChanged -= value;
        }

        public HierarchyTransform() : this(new ParentlessTransform()) { }

        public HierarchyTransform(Vector3 localPosition) : this(new ParentlessTransform(localPosition, Vector3.one, Quaternion.identity)) { }

        public HierarchyTransform(Vector3 localPosition, Vector3 localScale, Quaternion localRotation)
            : this(new ParentlessTransform(localPosition, localScale, localRotation)) { }

        public HierarchyTransform(ParentlessTransform core)
        {
            _core = core ?? throw new ArgumentNullException(nameof(core));
            _hasPendingOldWorld = false;
            _hasLastParent = false;
            RecalculateWorldAndPropagate();
        }
        
        public Vector3 LocalPosition
        {
            get => _core.LocalPosition;
            set => HandleLocalChange(
                () => _core.LocalPosition = value
            );
        }

        public Vector3 LocalScale
        {
            get => _core.LocalScale;
            set => HandleLocalChange(
                () => _core.LocalScale = value
            );
        }

        public Quaternion LocalRotation
        {
            get => _core.LocalRotation;
            set => HandleLocalChange(
                () => _core.LocalRotation = value
            );
        }
        
        private void HandleLocalChange(Action applyLocalChange)
        {
            (Vector3 pos, Vector3 scale, Quaternion rot) oldWorld = ComputeWorldUsingParent();
            applyLocalChange();
            (Vector3 pos, Vector3 scale, Quaternion rot) newWorld = ComputeWorldUsingParent();
            RecalculateWorldAndPropagate();

            if (oldWorld.pos != newWorld.pos)
                PositionChanged?.Invoke(oldWorld.pos, newWorld.pos);
            if (oldWorld.scale != newWorld.scale)
                LossyScaleChanged?.Invoke(oldWorld.scale, newWorld.scale);
            if (oldWorld.rot != newWorld.rot)
                RotationChanged?.Invoke(oldWorld.rot, newWorld.rot);
        }
        
        public Vector3 Position
        {
            get => ComputeWorldUsingParent().pos;
            set
            {
                (Vector3 pos, Vector3 scale, Quaternion rot) oldWorld = ComputeWorldUsingParent();
                Vector3 oldPos = oldWorld.pos;

                IHierarchyTransform parent = Parent as IHierarchyTransform;
                _core.SetLocalFromWorld(value, oldWorld.scale, oldWorld.rot,
                    parent?.Position, parent?.LossyScale, parent?.Rotation);

                (Vector3 pos, Vector3 scale, Quaternion rot) newWorld = ComputeWorldUsingParent();
                RecalculateWorldAndPropagate();

                if (oldPos != newWorld.pos)
                    PositionChanged?.Invoke(oldPos, newWorld.pos);
                if (oldWorld.scale != newWorld.scale)
                    LossyScaleChanged?.Invoke(oldWorld.scale, newWorld.scale);
                if (oldWorld.rot != newWorld.rot)
                    RotationChanged?.Invoke(oldWorld.rot, newWorld.rot);
            }
        }

        public Vector3 LossyScale => ComputeWorldUsingParent().scale;

        public Quaternion Rotation
        {
            get => ComputeWorldUsingParent().rot;
            set
            {
                (Vector3 pos, Vector3 scale, Quaternion rot) oldWorld = ComputeWorldUsingParent();
                Quaternion oldRot = oldWorld.rot;

                if (Parent is IHierarchyTransform parent)
                    _core.LocalRotation = Quaternion.Inverse(parent.Rotation) * value;
                else
                    _core.LocalRotation = value;

                (Vector3 pos, Vector3 scale, Quaternion rot) newWorld = ComputeWorldUsingParent();
                RecalculateWorldAndPropagate();

                if (oldRot != newWorld.rot)
                    RotationChanged?.Invoke(oldRot, newWorld.rot);
                if (oldWorld.pos != newWorld.pos)
                    PositionChanged?.Invoke(oldWorld.pos, newWorld.pos);
                if (oldWorld.scale != newWorld.scale)
                    LossyScaleChanged?.Invoke(oldWorld.scale, newWorld.scale);
            }
        }
        
        public void OnParentTransformChanged(Vector3 parentPosition, Vector3 parentScale, Quaternion parentRotation)
        {
            if (!_hasLastParent)
            {
                _lastParentPos = parentPosition;
                _lastParentScale = parentScale;
                _lastParentRot = parentRotation;
                _hasLastParent = true;

                RecalculateWorldAndPropagate();
                return;
            }

            Vector3 oldPos = _lastParentPos + _lastParentRot * Vector3.Scale(_core.LocalPosition, _lastParentScale);
            Vector3 oldScale = Vector3.Scale(_lastParentScale, _core.LocalScale);
            Quaternion oldRot = _lastParentRot * _core.LocalRotation;

            Vector3 newPos = parentPosition + parentRotation * Vector3.Scale(_core.LocalPosition, parentScale);
            Vector3 newScale = Vector3.Scale(parentScale, _core.LocalScale);
            Quaternion newRot = parentRotation * _core.LocalRotation;

            _lastParentPos = parentPosition;
            _lastParentScale = parentScale;
            _lastParentRot = parentRotation;

            if (oldPos != newPos)
                PositionChanged?.Invoke(oldPos, newPos);
            if (oldScale != newScale)
                LossyScaleChanged?.Invoke(oldScale, newScale);
            if (oldRot != newRot)
                RotationChanged?.Invoke(oldRot, newRot);

            RecalculateWorldAndPropagate();
        }

        private void RecalculateWorldAndPropagate()
        {
            (Vector3 pos, Vector3 scale, Quaternion rot) world = ComputeWorldUsingParent();
            foreach (IHierarchyElement child in Children)
            {
                if (child is IHierarchyTransform t)
                    t.OnParentTransformChanged(world.pos, world.scale, world.rot);
            }
        }

        private (Vector3 pos, Vector3 scale, Quaternion rot) ComputeWorldUsingParent()
        {
            IHierarchyTransform parent = Parent as IHierarchyTransform;
            
            if (parent == null)
                return (_core.LocalPosition, _core.LocalScale, _core.LocalRotation);

            Vector3 worldPos = parent.Position + parent.Rotation * Vector3.Scale(_core.LocalPosition, parent.LossyScale);
            Vector3 worldScale = Vector3.Scale(parent.LossyScale, _core.LocalScale);
            Quaternion worldRot = parent.Rotation * _core.LocalRotation;
            return (worldPos, worldScale, worldRot);
        }
        
        protected override void OnParentChanging(IHierarchyElement oldParent, IHierarchyElement newParent)
        {
            if (oldParent is IHierarchyTransform oldT)
            {
                Vector3 oldPos = oldT.Position + oldT.Rotation * Vector3.Scale(_core.LocalPosition, oldT.LossyScale);
                Vector3 oldScale = Vector3.Scale(oldT.LossyScale, _core.LocalScale);
                Quaternion oldRot = oldT.Rotation * _core.LocalRotation;

                _pendingOldWorldPos = oldPos;
                _pendingOldWorldScale = oldScale;
                _pendingOldWorldRot = oldRot;
            }
            else
            {
                _pendingOldWorldPos = _core.LocalPosition;
                _pendingOldWorldScale = _core.LocalScale;
                _pendingOldWorldRot = _core.LocalRotation;
            }
            _hasPendingOldWorld = true;
        }

        protected override void OnParentSet(IHierarchyElement parent)
        {
            if (_hasPendingOldWorld)
            {
                if (parent is IHierarchyTransform pt)
                {
                    _core.SetLocalFromWorld(_pendingOldWorldPos, _pendingOldWorldScale, _pendingOldWorldRot,
                        pt.Position, pt.LossyScale, pt.Rotation);
                }
                else
                {
                    _core.SetLocalFromWorld(_pendingOldWorldPos, _pendingOldWorldScale, _pendingOldWorldRot,
                        null, null, null);
                }
                _hasPendingOldWorld = false;
            }

            if (parent is IHierarchyTransform ip)
            {
                _lastParentPos = ip.Position;
                _lastParentScale = ip.LossyScale;
                _lastParentRot = ip.Rotation;
                _hasLastParent = true;
            }
            else
            {
                _hasLastParent = false;
            }

            RecalculateWorldAndPropagate();
        }

        protected override void OnChildAdded(IHierarchyElement child)
        {
            if (child is IHierarchyTransform t)
            {
                (Vector3 pos, Vector3 scale, Quaternion rot) world = ComputeWorldUsingParent();
                t.OnParentTransformChanged(world.pos, world.scale, world.rot);
            }
        }
    }
}