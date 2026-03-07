using UnityEngine;

namespace IM.Visuals
{
    [DisallowMultipleComponent]
    public sealed class TransformFloatSetter : MonoBehaviour
    {
        public float PositionX
        {
            set
            {
                var p = transform.localPosition;
                p.x = value;
                transform.localPosition = p;
            }
        }

        public float PositionY
        {
            set
            {
                var p = transform.localPosition;
                p.y = value;
                transform.localPosition = p;
            }
        }

        public float PositionZ
        {
            set
            {
                var p = transform.localPosition;
                p.z = value;
                transform.localPosition = p;
            }
        }

        public float RotationX
        {
            set
            {
                var r = transform.localEulerAngles;
                r.x = value;
                transform.localEulerAngles = r;
            }
        }

        public float RotationY
        {
            set
            {
                var r = transform.localEulerAngles;
                r.y = value;
                transform.localEulerAngles = r;
            }
        }

        public float RotationZ
        {
            set
            {
                var r = transform.localEulerAngles;
                r.z = value;
                transform.localEulerAngles = r;
            }
        }

        public float ScaleX
        {
            set
            {
                var s = transform.localScale;
                s.x = value;
                transform.localScale = s;
            }
        }

        public float ScaleY
        {
            set
            {
                var s = transform.localScale;
                s.y = value;
                transform.localScale = s;
            }
        }

        public float ScaleZ
        {
            set
            {
                var s = transform.localScale;
                s.z = value;
                transform.localScale = s;
            }
        }
    }
}