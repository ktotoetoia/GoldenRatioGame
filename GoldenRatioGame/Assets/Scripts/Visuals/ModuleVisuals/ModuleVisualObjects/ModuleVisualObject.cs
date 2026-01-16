using System;
using System.Collections.Generic;
using IM.Graphs;
using IM.Transforms;
using UnityEngine;

namespace IM.Visuals
{
    public class ModuleVisualObject : MonoBehaviour, IModuleVisualObjectAnimated
    {
        [SerializeField] bool _isVisibleOnAwake;
        private readonly Dictionary<IPort, IHierarchyTransform> _portsTransforms =  new();
        private readonly HierarchyTransform _transform= new();
        private Animator _animator;
        
        public bool Visibility
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }
        public IModule Owner { get; set; }
        public IHierarchyTransform Transform => _transform;
        public IReadOnlyDictionary<IPort, IHierarchyTransform> PortsTransforms => _portsTransforms;

        public Animator Animator => _animator??= GetComponent<Animator>();
        
        private void Awake()
        {
            Visibility = _isVisibleOnAwake;
            _transform.PositionChanged += (_, newValue) => transform.position = newValue;
            _transform.LossyScaleChanged += (_, newValue) => transform.localScale = newValue;
            _transform.RotationChanged += (_, newValue) => transform.rotation = newValue;
        }
        
        public void AddPort(IPort port, IHierarchyTransform portTransform)
        {
            _portsTransforms[port] = portTransform;
        }

        public void Dispose()
        {
            _transform.SetParent(null);
            Destroy(gameObject);
        }
        
        public void ResetTransform()
        {
            _transform.SetParent(null);
            _transform.LocalPosition = default;
            _transform.LocalRotation = default;
            _transform.LocalScale = default;
        }
    }
}