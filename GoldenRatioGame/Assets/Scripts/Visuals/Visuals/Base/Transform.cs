using UnityEngine;

namespace IM.Visuals
{
    public class Transform : ITransform
    {
        private readonly ITransform _parent;
        
        public Vector3 LocalPosition { get; set; }
        public Vector3 LocalScale { get; set; }
        public Quaternion LocalRotation { get; set; }
        
        public Vector3 Position
        {
            get => _parent.Position + _parent.Rotation * Vector3.Scale(LocalPosition, _parent.Scale);
            set => LocalPosition = Quaternion.Inverse(_parent.Rotation) * (value - _parent.Position) / _parent.Scale.x;
        }

        public Vector3 Scale
        {
            get => Vector3.Scale(_parent.Scale, LocalScale);
            set => LocalScale = new Vector3(
                value.x / _parent.Scale.x,
                value.y / _parent.Scale.y,
                value.z / _parent.Scale.z
            );
        }

        public Quaternion Rotation
        {
            get => _parent.Rotation * LocalRotation;
            set => LocalRotation = Quaternion.Inverse(_parent.Rotation) * value;
        }
        
        public Transform(ITransform parent) : this(parent, Vector3.zero, Vector3.one, Quaternion.identity) { }
        public Transform(ITransform parent,Vector3 localPosition) : this(parent, localPosition, Vector3.one, Quaternion.identity) { }
        
        public Transform(Vector3 localPosition) : this(new DefaultTransform(), localPosition, Vector3.one, Quaternion.identity) { }
        public Transform(Vector3 localPosition, Vector3 localScale, Quaternion localRotation)
            : this(new DefaultTransform(), localPosition, localScale, localRotation) { }

        public Transform(ITransform parent, Vector3 localPosition, Vector3 localScale, Quaternion localRotation)
        {
            _parent = parent ?? new DefaultTransform();
            LocalPosition = localPosition;
            LocalScale = localScale;
            LocalRotation = localRotation;
        }
    }
}