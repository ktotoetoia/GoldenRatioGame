using UnityEngine;

namespace IM.Visuals
{
    [DisallowMultipleComponent]
    public sealed class TransformFloatSetter : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        private Transform Target => _target != null ? _target : transform;

        public float PositionX
        {
            set
            {
                var p = Target.localPosition;
                p.x = value;
                Target.localPosition = p;
            }
        }

        public float PositionY
        {
            set
            {
                var p = Target.localPosition;
                p.y = value;
                Target.localPosition = p;
            }
        }

        public float PositionZ
        {
            set
            {
                var p = Target.localPosition;
                p.z = value;
                Target.localPosition = p;
            }
        }

        public float RotationX
        {
            set
            {
                var r = Target.localEulerAngles;
                r.x = value;
                Target.localEulerAngles = r;
            }
        }

        public float RotationY
        {
            set
            {
                var r = Target.localEulerAngles;
                r.y = value;
                Target.localEulerAngles = r;
            }
        }

        public float RotationZ
        {
            set
            {
                var r = Target.localEulerAngles;
                r.z = value;
                Target.localEulerAngles = r;
            }
        }

        public float ScaleX
        {
            set
            {
                var s = Target.localScale;
                s.x = value;
                Target.localScale = s;
            }
        }

        public float ScaleY
        {
            set
            {
                var s = Target.localScale;
                s.y = value;
                Target.localScale = s;
            }
        }

        public float ScaleZ
        {
            set
            {
                var s = Target.localScale;
                s.z = value;
                Target.localScale = s;
            }
        }
    }
}