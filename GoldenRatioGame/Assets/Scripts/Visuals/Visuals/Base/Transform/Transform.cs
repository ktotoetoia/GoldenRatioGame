using System;
using System.Collections.Generic;
using UnityEngine;

namespace IM.Visuals
{
    public class Transform : ITransform
    {
        public ITransform Parent => _hierarchy.Parent;
        public IReadOnlyList<ITransform> Children => _hierarchy.Children;

        private readonly Hierarchy<ITransform> _hierarchy;
        private Vector3 _cachedParentPosition;
        private Vector3 _cachedParentScale;
        private Quaternion _cachedParentRotation;
        private Vector3 _localPosition;
        private Vector3 _localScale;
        private Quaternion _localRotation;
        private Vector3 _worldPosition;
        private Vector3 _worldScale;
        private Quaternion _worldRotation;

        public Transform() : this(Vector3.zero, Vector3.one, Quaternion.identity) { }

        public Transform(Vector3 localPosition) : this(localPosition, Vector3.one, Quaternion.identity) { }

        public Transform(Vector3 localPosition, Vector3 localScale, Quaternion localRotation)
        {
            _hierarchy = new Hierarchy<ITransform>(this);
            _localPosition = localPosition;
            _localScale = localScale;
            _localRotation = localRotation;
            _cachedParentPosition = Vector3.zero;
            _cachedParentScale = Vector3.one;
            _cachedParentRotation = Quaternion.identity;
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
            get => _worldPosition;
            set
            {
                Quaternion inverseParentRot = Quaternion.Inverse(_cachedParentRotation);
                Vector3 tmp = inverseParentRot * (value - _cachedParentPosition);
                _localPosition = new Vector3(
                    SafeDiv(tmp.x, _cachedParentScale.x),
                    SafeDiv(tmp.y, _cachedParentScale.y),
                    SafeDiv(tmp.z, _cachedParentScale.z)
                );
                RecalculateWorldAndPropagate();
            }
        }

        public Vector3 Scale
        {
            get => _worldScale;
            set
            {
                _localScale = new Vector3(
                    SafeDiv(value.x, _cachedParentScale.x),
                    SafeDiv(value.y, _cachedParentScale.y),
                    SafeDiv(value.z, _cachedParentScale.z)
                );
                RecalculateWorldAndPropagate();
            }
        }

        public Quaternion Rotation
        {
            get => _worldRotation;
            set
            {
                _localRotation = Quaternion.Inverse(_cachedParentRotation) * value;
                RecalculateWorldAndPropagate();
            }
        }

        public void SetParent(ITransform newParent, bool keepWorld = true)
        {
            if (newParent == _hierarchy.Parent) return;
            if (newParent == this) throw new ArgumentException("Cannot set parent to self.", nameof(newParent));
            ITransform walker = newParent;
            while (walker != null)
            {
                if (walker == this) throw new ArgumentException("Cannot set parent to a descendant (would create cycle).", nameof(newParent));
                walker = walker.Parent;
            }

            Vector3 oldWorldPos = _worldPosition;
            Vector3 oldWorldScale = _worldScale;
            Quaternion oldWorldRot = _worldRotation;

            ITransform oldParent = _hierarchy.Parent;
            if (oldParent != null)
            {
                bool removed = oldParent.RemoveChildInternal(this);
                bool removedUnused = removed;
            }

            _hierarchy.SetParentInternal(newParent);

            if (newParent != null)
            {
                if (!newParent.Contains(this))
                {
                    newParent.AddChildInternal(this);
                }

                if (keepWorld)
                {
                    Quaternion inv = Quaternion.Inverse(newParent.Rotation);
                    Vector3 relPos = inv * (oldWorldPos - newParent.Position);
                    _localPosition = new Vector3(
                        SafeDiv(relPos.x, newParent.Scale.x),
                        SafeDiv(relPos.y, newParent.Scale.y),
                        SafeDiv(relPos.z, newParent.Scale.z)
                    );

                    _localScale = new Vector3(
                        SafeDiv(oldWorldScale.x, newParent.Scale.x),
                        SafeDiv(oldWorldScale.y, newParent.Scale.y),
                        SafeDiv(oldWorldScale.z, newParent.Scale.z)
                    );

                    _localRotation = Quaternion.Inverse(newParent.Rotation) * oldWorldRot;
                }

                OnParentTransformChanged(newParent.Position, newParent.Scale, newParent.Rotation);
            }
            else
            {
                if (keepWorld)
                {
                    _localPosition = oldWorldPos;
                    _localScale = oldWorldScale;
                    _localRotation = oldWorldRot;
                }

                OnParentTransformChanged(Vector3.zero, Vector3.one, Quaternion.identity);
            }
        }

        public void AddChild(ITransform child)
        {
            if (child == null) throw new ArgumentNullException(nameof(child));
            child.SetParent(this, true);
        }

        public bool RemoveChild(ITransform child)
        {
            if (child == null) return false;
            if (child.Parent != this) return false;
            child.SetParent(null, true);
            return true;
        }

        public bool Contains(ITransform child)
        {
            if (child == null) return false;
            return _hierarchy.Contains(child);
        }

        public void AddChildInternal(ITransform child)
        {
            _hierarchy.AddChildInternal(child);
        }

        public bool RemoveChildInternal(ITransform child)
        {
            return _hierarchy.RemoveChildInternal(child);
        }

        public void OnParentTransformChanged(Vector3 parentPosition, Vector3 parentScale, Quaternion parentRotation)
        {
            _cachedParentPosition = parentPosition;
            _cachedParentScale = parentScale;
            _cachedParentRotation = parentRotation;
            RecalculateWorldAndPropagate();
        }

        private void RecalculateWorldAndPropagate()
        {
            RecalculateWorldFromLocalAndParent();
            foreach (ITransform child in _hierarchy.Children)
            {
                child.OnParentTransformChanged(_worldPosition, _worldScale, _worldRotation);
            }
        }

        private void RecalculateWorldFromLocalAndParent()
        {
            _worldPosition = _cachedParentPosition + _cachedParentRotation * Vector3.Scale(_localPosition, _cachedParentScale);
            _worldScale = Vector3.Scale(_cachedParentScale, _localScale);
            _worldRotation = _cachedParentRotation * _localRotation;
        }

        private static float SafeDiv(float a, float b)
        {
            if (Mathf.Approximately(b, 0f)) return 0f;
            return a / b;
        }
    }
}