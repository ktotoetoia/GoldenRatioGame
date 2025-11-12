using UnityEngine;

namespace IM.Visuals
{
    public class DefaultTransform : ITransform
    {
        public Vector3 Position
        {
            get=> LocalPosition;
            set => LocalPosition = value;
        }

        public Vector3 Scale
        {
            get => LocalScale;
            set => LocalScale = value;
        }

        public Quaternion Rotation
        {
            get => LocalRotation;
            set => LocalRotation = value;
        }

        public Vector3 LocalPosition { get; set; } = new(0,0,0);
        public Vector3 LocalScale { get; set; } = new(1,1,1);
        public Quaternion LocalRotation { get; set; } =  new (0,0,0,1);
    }
}