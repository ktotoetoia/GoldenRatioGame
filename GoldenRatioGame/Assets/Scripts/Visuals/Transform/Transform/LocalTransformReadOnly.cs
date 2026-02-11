using UnityEngine;

namespace IM.Transforms
{
    public readonly struct LocalTransformReadOnly : ITransformReadOnly
    {
        public Vector3 Position { get; }
        public Vector3 LocalPosition => Vector3.zero;
        public Vector3 LossyScale { get; }
        public Vector3 LocalScale => Vector3.one;
        public Quaternion Rotation { get; }
        public Quaternion LocalRotation => Quaternion.identity;
    
        public LocalTransformReadOnly(Vector3 p, Quaternion r,Vector3 s)
        {
            Position = p;
            LossyScale = s;
            Rotation = r;
        }

        public void ApplyTo(ITransform target)
        {
            target.LocalPosition = Position;
            target.LocalRotation = Rotation;
            target.LocalScale = LossyScale;
        }
        
        public static LocalTransformReadOnly Default = new (Vector3.zero, Quaternion.identity, Vector3.one);
    }
}