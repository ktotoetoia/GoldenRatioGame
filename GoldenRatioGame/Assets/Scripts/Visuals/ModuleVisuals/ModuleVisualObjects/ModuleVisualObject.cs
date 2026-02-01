using System;
using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.Modules;
using IM.Transforms;
using UnityEngine;

namespace IM.Visuals
{
    public class ModuleVisualObject : MonoBehaviour, IAnimatedModuleVisualObject
    {
        private readonly List<IPortVisualObject> _portVisualObjects = new();
        private readonly HierarchyTransform _transform = new();
        private Animator _animator;
        private IExtensibleModule _owner;

        public IHierarchyTransform Transform => _transform;
        public IReadOnlyList<IPortVisualObject> PortsVisualObjects => _portVisualObjects;
        public IExtensibleModule Owner
        {
            get => _owner;
            set
            {
                if(_owner != null) throw new InvalidOperationException("Owner can only be set once");
                
                _owner = value;
            }
        }

        public bool IsAnimating { get; set; } = true;
        public IEnumerable<IAnimationChange> AnimationChanges { get; set; }
        public IModuleGraphStructureUpdater ModuleGraphStructureUpdater { get; set; }

        public bool Visibility
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();

            transform.position = _transform.Position;
            transform.rotation  = _transform.Rotation;
            transform.localScale = _transform.LossyScale;
            _transform.PositionChanged += (_, newValue) => transform.position = newValue;
            _transform.LossyScaleChanged += (_, newValue) => transform.localScale = newValue;
            _transform.RotationChanged += (_, newValue) => transform.rotation = newValue;
        }

        private void Update()
        {
            if (AnimationChanges == null || !IsAnimating) return;
         
            foreach (IAnimationChange animationChange in AnimationChanges)
            {
                animationChange.ApplyToAnimator(_animator);
            }
        }
        
        public void AddPort(IPortVisualObject port, IHierarchyTransform hierarchyTransform)
        {
            _portVisualObjects.Add(port);
            _transform.AddChildKeepLocal(hierarchyTransform);
        }
        
        public IPortVisualObject GetPortVisualObject(IPort port)
        {
            return _portVisualObjects.FirstOrDefault(x => x.Port == port);
        }

        public void OnInitializationFinished()
        {
            foreach (IRequireModuleVisualObjectInitialization initialization in GetComponents<IRequireModuleVisualObjectInitialization>())
            {
                initialization.OnModuleVisualObjectInitialized(this);
            }
        }

        public void Dispose()
        {
            Reset();
            Destroy(gameObject);
        }
        
        public void Reset()
        {
            Visibility = false;
            ModuleGraphStructureUpdater = null;
            _transform.SetParent(null);
            _transform.LocalPosition = new Vector3(0, 0, 0);
            _transform.LocalScale = new Vector3(1, 1, 1);
            _transform.LocalRotation = Quaternion.identity;
        }
    }
}