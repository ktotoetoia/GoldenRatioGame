using System;
using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using UnityEngine;

namespace IM.Visuals
{
    public class AnimationModule : MonoBehaviour,IAnimationModule
    {
        private Animator _animator;
        private readonly List<ITransformPort> _ports = new();
        private HierarchyTransform _hierarchyTransform;
        private Sprite _icon;
        private bool _initialized;
        private ITransformPort _anchorPort;
        
        public Animator Animator => _animator??= GetComponent<Animator>();
        public IEnumerable<IEdge> Edges => _ports.Where(x => x.IsConnected).Select(x => x.Connection);
        public IEnumerable<ITransformPort> Ports => _ports;
        IEnumerable<IPort> IModule.Ports => _ports;
        public IHierarchyTransformReadOnly HierarchyTransform
        {
            get
            {
                if (_hierarchyTransform == null) Initialize();
                
                return _hierarchyTransform;
            }  
        }

        private void Initialize()
        {
            if (_initialized) throw new Exception(); 
            
            _hierarchyTransform = new();
            
            _hierarchyTransform.PositionChanged += (_, newValue) => transform.position = newValue;
            _hierarchyTransform.LossyScaleChanged += (_, newValue) => transform.localScale = newValue;
            _hierarchyTransform.RotationChanged += (_, newValue) => transform.localRotation = newValue;
            
            _initialized = true;
        }

        public void AddPort(ITransformPort port) => _ports.Add(port);
        public void Dispose() => Destroy(gameObject);
    }
}
