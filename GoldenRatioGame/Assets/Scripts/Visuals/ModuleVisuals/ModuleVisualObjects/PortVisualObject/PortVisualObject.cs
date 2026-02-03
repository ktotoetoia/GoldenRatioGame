using IM.Graphs;
using IM.Transforms;
using UnityEngine;

namespace IM.Visuals
{
    public class PortVisualObject : IPortVisualObject
    {
        private readonly Vector3 _defaultPosition;
        private readonly Quaternion _defaultRotation;
        private readonly Vector3 _defaultScale;
        
        public IModuleVisualObject OwnerVisualObject { get; }
        public IPort Port { get; }
        public ITransform Transform { get; }

        public bool Visibility { get; set; }
        
        public PortVisualObject(IModuleVisualObject ownerVisualObject, IPort port, ITransform transform)
        {
            OwnerVisualObject = ownerVisualObject;
            Port = port;
            Transform = transform;
            
            _defaultPosition = Transform.LocalPosition;
            _defaultRotation = Transform.LocalRotation;
            _defaultScale = Transform.LocalScale;
        }
        
        public void Reset()
        {
            Transform.LocalPosition = _defaultPosition;
            Transform.LocalRotation = _defaultRotation;
            Transform.LocalScale = _defaultScale;
        }

        public void Dispose() => Visibility = false;
    }
}