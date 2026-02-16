using System;
using UnityEngine;

namespace IM.Transforms
{
    [Serializable]
    public struct LocalTransformPreset : ITransformReadOnly
    {
        [SerializeField] private bool _changePosition;
        [SerializeField] private bool _changeRotation;
        [SerializeField] private bool _changeScale;
        [SerializeField] private Vector3 _position;
        [SerializeField] private Quaternion _rotation;
        [SerializeField] private Vector3 _scale;

        public Vector3 Position => _position;
        public Vector3 LocalPosition => _position;
        public Vector3 LossyScale => _scale;
        public Vector3 LocalScale => _scale;
        public Quaternion Rotation => _rotation;
        public Quaternion LocalRotation => _rotation;
    
        public static LocalTransformPreset Default = new (Vector3.zero, Quaternion.identity, Vector3.one);
        
        public LocalTransformPreset(Vector3 p, Quaternion r,Vector3 s)
        {
            _position = p;
            _scale = s;
            _rotation = r;
            _changePosition = true;
            _changeRotation = true;
            _changeScale = true;
        }

        public void ApplyTo(ITransform target)
        {
            if(_changePosition) target.LocalPosition = Position;
            if(_changeRotation) target.LocalRotation = Rotation;
            if(_changeScale) target.LocalScale = LossyScale;
        }
    }
}