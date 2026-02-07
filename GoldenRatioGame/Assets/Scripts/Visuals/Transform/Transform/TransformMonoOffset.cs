using UnityEngine;

namespace IM.Transforms
{
    public class TransformMonoOffset : MonoBehaviour, ITransform
    {
        [field: SerializeField] public Vector3 PositionOffset { get; set; }
        [field: SerializeField] public Quaternion RotationOffset { get; set; }

        public Vector3 Position
        {
            get => transform.TransformPoint(-PositionOffset);
            set => transform.position = value + transform.TransformVector(PositionOffset);
        }

        public Vector3 LocalPosition
        {
            get => transform.localPosition - PositionOffset;
            set => transform.localPosition = value + PositionOffset;
        }

        public Vector3 LossyScale => transform.lossyScale;

        public Vector3 LocalScale
        {
            get => transform.localScale;
            set => transform.localScale = value;
        }

        public Quaternion Rotation
        {
            get => transform.rotation * Quaternion.Inverse(RotationOffset);
            set => transform.rotation = value * RotationOffset;
        }

        public Quaternion LocalRotation
        {
            get => transform.localRotation * Quaternion.Inverse(RotationOffset);
            set => transform.localRotation = value * RotationOffset;
        }

        public Transform Transform => transform;
    }
}