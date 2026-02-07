using UnityEngine;

namespace IM.Transforms
{
    public class TransformMono : MonoBehaviour, ITransform
    {
        public Vector3 Position
        {
            get => transform.position; 
            set => transform.position = value; 
        }
        public Vector3 LocalPosition
        {
            get => transform.localPosition;
            set => transform.localPosition = value;
        }
        public Vector3 LossyScale => transform.lossyScale;
        public Vector3 LocalScale 
        {
            get => transform.localScale;
            set => transform.localScale = value;
        }
        public Quaternion Rotation
        {
            get => transform.rotation;
            set => transform.rotation = value;
        }
        public Quaternion LocalRotation
        {
            get=> transform.localRotation;
            set => transform.localRotation = value;
        }

        public Transform Transform => transform;
    }
}