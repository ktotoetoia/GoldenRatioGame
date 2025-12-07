using System;
using UnityEngine;

namespace IM.Visuals
{
    public class TransformCore
    {
        private Vector3 _localPosition;
        private Vector3 _localScale;
        private Quaternion _localRotation;

        public event Action<Vector3, Vector3> LocalPositionChanged;
        public event Action<Vector3, Vector3> LocalScaleChanged;
        public event Action<Quaternion, Quaternion> LocalRotationChanged;

        public TransformCore() : this(Vector3.zero, Vector3.one, Quaternion.identity) { }

        public TransformCore(Vector3 localPosition, Vector3 localScale, Quaternion localRotation)
        {
            _localPosition = localPosition;
            _localScale = localScale;
            _localRotation = localRotation;
        }

        public Vector3 LocalPosition
        {
            get => _localPosition;
            set
            {
                Vector3 old = _localPosition;
                if (old == value) return;
                _localPosition = value;
                LocalPositionChanged?.Invoke(old, value);
            }
        }

        public Vector3 LocalScale
        {
            get => _localScale;
            set
            {
                Vector3 old = _localScale;
                if (old == value) return;
                _localScale = value;
                LocalScaleChanged?.Invoke(old, value);
            }
        }

        public Quaternion LocalRotation
        {
            get => _localRotation;
            set
            {
                Quaternion old = _localRotation;
                if (old == value) return;
                _localRotation = value;
                LocalRotationChanged?.Invoke(old, value);
            }
        }

        public (Vector3 pos, Vector3 scale, Quaternion rot) ComputeWorld(
            Vector3? parentPosition,
            Vector3? parentScale,
            Quaternion? parentRotation)
        {
            if (!parentPosition.HasValue || !parentScale.HasValue || !parentRotation.HasValue)
                return (_localPosition, _localScale, _localRotation);

            Vector3 pPos = parentPosition.Value;
            Vector3 pScale = parentScale.Value;
            Quaternion pRot = parentRotation.Value;

            Vector3 worldPos = pPos + pRot * Vector3.Scale(_localPosition, pScale);
            Vector3 worldScale = Vector3.Scale(pScale, _localScale);
            Quaternion worldRot = pRot * _localRotation;
            return (worldPos, worldScale, worldRot);
        }

        public void SetLocalFromWorld(Vector3 worldPos, Vector3 worldScale, Quaternion worldRot,
            Vector3? parentPosition,
            Vector3? parentScale,
            Quaternion? parentRotation)
        {
            if (!parentPosition.HasValue || !parentScale.HasValue || !parentRotation.HasValue)
            {
                LocalPosition = worldPos;
                LocalScale = worldScale;
                LocalRotation = worldRot;
            }
            else
            {
                Vector3 pPos = parentPosition.Value;
                Vector3 pScale = parentScale.Value;
                Quaternion pRot = parentRotation.Value;

                Quaternion inv = Quaternion.Inverse(pRot);
                Vector3 relPos = inv * (worldPos - pPos);

                LocalPosition = new Vector3(
                    SafeDiv(relPos.x, pScale.x),
                    SafeDiv(relPos.y, pScale.y),
                    SafeDiv(relPos.z, pScale.z)
                );

                LocalScale = new Vector3(
                    SafeDiv(worldScale.x, pScale.x),
                    SafeDiv(worldScale.y, pScale.y),
                    SafeDiv(worldScale.z, pScale.z)
                );

                LocalRotation = Quaternion.Inverse(pRot) * worldRot;
            }
        }

        private static float SafeDiv(float a, float b)
        {
            if (Mathf.Approximately(b, 0f)) return 0f;
            return a / b;
        }
    }
}