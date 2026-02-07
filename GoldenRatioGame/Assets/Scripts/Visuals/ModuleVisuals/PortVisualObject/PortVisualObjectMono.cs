using System;
using IM.Graphs;
using IM.Transforms;
using UnityEngine;

namespace IM.Visuals
{
    public class PortVisualObjectMono : MonoBehaviour, IPortVisualObject
    {
        private LocalTransformReadOnly _localTransformReadOnly;
        
        public bool Visible { get; set; }
        public IModuleVisualObject OwnerVisualObject { get; private set; }
        public IPort Port { get; private set; }
        public ITransform Transform { get; private set; }
        public int OutputOrderAdjustment { get; set; }

        public LocalTransformReadOnly LocalTransformReadOnly
        {
            get=> _localTransformReadOnly;
            set
            {
                _localTransformReadOnly = value;
                _localTransformReadOnly.ApplyTo(Transform);
            }
        }

        private void Awake()
        {
            Transform = GetComponent<ITransform>();
        }

        public void Initialize(IModuleVisualObject ownerVisualObject, IPort port)
        {
            if (OwnerVisualObject != null && Port != null) throw new InvalidOperationException("PortVisualObject is already initialized");
            
            OwnerVisualObject = ownerVisualObject ?? throw new ArgumentNullException(nameof(ownerVisualObject));
            Port = port ??  throw new ArgumentNullException(nameof(port));
        }
        
        public void Reset()
        {
            _localTransformReadOnly.ApplyTo(Transform);
        }
        
        public void Dispose()
        {
            Destroy(gameObject);
        }
    }
}