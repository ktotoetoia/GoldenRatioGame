using UnityEngine;

namespace IM.Transforms
{
    public struct TransformReadOnly : ITransformReadOnly
    {
        public Vector3 Position { get; }
        public Vector3 LocalPosition => Vector3.zero;
        public Vector3 LossyScale { get; }
        public Vector3 LocalScale => Vector3.one;
        public Quaternion Rotation { get; }
        public Quaternion LocalRotation => Quaternion.identity;
    
        public TransformReadOnly(Vector3 p, Vector3 s, Quaternion r)
        {
            Position = p;
            LossyScale = s;
            Rotation = r;
        }
    }
}