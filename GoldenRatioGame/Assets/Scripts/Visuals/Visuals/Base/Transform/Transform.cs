using System.Collections.Generic;
using UnityEngine;
using IM.Graphs;

namespace IM.Visuals
{
    public class Transform : HierarchyElement, ITransform
    {
        private Vector3 _localPosition;
        private Vector3 _localScale;
        private Quaternion _localRotation;

        private bool _hasPendingOldWorld;
        private Vector3 _pendingOldWorldPos;
        private Vector3 _pendingOldWorldScale;
        private Quaternion _pendingOldWorldRot;

        public Transform() : this(Vector3.zero, Vector3.one, Quaternion.identity) { }

        public Transform(Vector3 localPosition) : this(localPosition, Vector3.one, Quaternion.identity) { }

        public Transform(Vector3 localPosition, Vector3 localScale, Quaternion localRotation)
        {
            _localPosition = localPosition;
            _localScale = localScale;
            _localRotation = localRotation;
            _hasPendingOldWorld = false;
            RecalculateWorldAndPropagate();
        }

        public Vector3 LocalPosition
        {
            get => _localPosition;
            set
            {
                _localPosition = value;
                RecalculateWorldAndPropagate();
            }
        }

        public Vector3 LocalScale
        {
            get => _localScale;
            set
            {
                _localScale = value;
                RecalculateWorldAndPropagate();
            }
        }

        public Quaternion LocalRotation
        {
            get => _localRotation;
            set
            {
                _localRotation = value;
                RecalculateWorldAndPropagate();
            }
        }

        public Vector3 Position
        {
            get => ComputeWorldUsingParent().pos;
            set
            {
                var parent = Parent as ITransform;
                SetLocalFromWorld(value, ComputeWorldUsingParent().scale, ComputeWorldUsingParent().rot, parent);
                RecalculateWorldAndPropagate();
            }
        }

        public Vector3 Scale
        {
            get => ComputeWorldUsingParent().scale;
            set
            {
                var parent = Parent as ITransform;
                SetLocalFromWorld(ComputeWorldUsingParent().pos, value, ComputeWorldUsingParent().rot, parent);
                RecalculateWorldAndPropagate();
            }
        }

        public Quaternion Rotation
        {
            get => ComputeWorldUsingParent().rot;
            set
            {
                var parent = Parent as ITransform;
                if (parent != null)
                    _localRotation = Quaternion.Inverse(parent.Rotation) * value;
                else
                    _localRotation = value;
                RecalculateWorldAndPropagate();
            }
        }

        public void OnParentTransformChanged(Vector3 parentPosition, Vector3 parentScale, Quaternion parentRotation)
        {
            RecalculateWorldAndPropagate();
        }

        private void RecalculateWorldAndPropagate()
        {
            (Vector3 pos, Vector3 scale, Quaternion rot) world = ComputeWorldUsingParent();

            foreach (IHierarchyElement child in Children)
            {
                if (child is ITransform t)
                    t.OnParentTransformChanged(world.pos, world.scale, world.rot);
            }
        }

        private (Vector3 pos, Vector3 scale, Quaternion rot) ComputeWorldUsingParent()
        {
            var parent = Parent as ITransform;
            if (parent == null)
            {
                return (_localPosition, _localScale, _localRotation);
            }

            Vector3 parentPos = parent.Position;
            Vector3 parentScale = parent.Scale;
            Quaternion parentRot = parent.Rotation;

            Vector3 worldPos = parentPos + parentRot * Vector3.Scale(_localPosition, parentScale);
            Vector3 worldScale = Vector3.Scale(parentScale, _localScale);
            Quaternion worldRot = parentRot * _localRotation;
            return (worldPos, worldScale, worldRot);
        }

        private static float SafeDiv(float a, float b)
        {
            if (Mathf.Approximately(b, 0f)) return 0f;
            return a / b;
        }

        protected override void OnParentChanging(IHierarchyElement oldParent, IHierarchyElement newParent)
        {
            if (oldParent is ITransform optOld)
            {
                Vector3 oldPos = optOld.Position + optOld.Rotation * Vector3.Scale(_localPosition, optOld.Scale);
                Vector3 oldScale = Vector3.Scale(optOld.Scale, _localScale);
                Quaternion oldRot = optOld.Rotation * _localRotation;
                _pendingOldWorldPos = oldPos;
                _pendingOldWorldScale = oldScale;
                _pendingOldWorldRot = oldRot;
            }
            else
            {
                _pendingOldWorldPos = _localPosition;
                _pendingOldWorldScale = _localScale;
                _pendingOldWorldRot = _localRotation;
            }
            _hasPendingOldWorld = true;
        }

        protected override void OnParentSet(IHierarchyElement parent)
        {
            if (_hasPendingOldWorld)
            {
                if (parent is ITransform pt)
                {
                    SetLocalFromWorld(_pendingOldWorldPos, _pendingOldWorldScale, _pendingOldWorldRot, pt);
                }
                else
                {
                    _localPosition = _pendingOldWorldPos;
                    _localScale = _pendingOldWorldScale;
                    _localRotation = _pendingOldWorldRot;
                }

                _hasPendingOldWorld = false;
            }

            RecalculateWorldAndPropagate();
        }

        protected override void OnChildAdded(IHierarchyElement child)
        {
            if (child is ITransform t)
            {
                var world = ComputeWorldUsingParent();
                t.OnParentTransformChanged(world.pos, world.scale, world.rot);
            }
        }

        private void SetLocalFromWorld(Vector3 worldPos, Vector3 worldScale, Quaternion worldRot, ITransform parent)
        {
            if (parent != null)
            {
                Quaternion inv = Quaternion.Inverse(parent.Rotation);
                Vector3 relPos = inv * (worldPos - parent.Position);
                _localPosition = new Vector3(
                    SafeDiv(relPos.x, parent.Scale.x),
                    SafeDiv(relPos.y, parent.Scale.y),
                    SafeDiv(relPos.z, parent.Scale.z)
                );

                _localScale = new Vector3(
                    SafeDiv(worldScale.x, parent.Scale.x),
                    SafeDiv(worldScale.y, parent.Scale.y),
                    SafeDiv(worldScale.z, parent.Scale.z)
                );

                _localRotation = Quaternion.Inverse(parent.Rotation) * worldRot;
            }
            else
            {
                _localPosition = worldPos;
                _localScale = worldScale;
                _localRotation = worldRot;
            }
        }
    }
}