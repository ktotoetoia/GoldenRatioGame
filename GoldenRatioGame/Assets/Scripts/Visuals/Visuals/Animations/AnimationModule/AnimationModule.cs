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
        private readonly List<IVisualPort> _ports = new();
        private HierarchyTransform _hierarchyTransform;
        private SpriteRenderer _renderer;
        private Sprite _icon;
        private bool _initialized;
        
        public Animator Animator => _animator??= GetComponent<Animator>();
        public IEnumerable<IEdge> Edges => _ports.Where(x => x.IsConnected).Select(x => x.Connection);
        public IEnumerable<IVisualPort> Ports => _ports;
        IEnumerable<IPort> IModule.Ports => _ports;
        public IHierarchyTransform HierarchyTransform
        {
            get
            {
                if (_hierarchyTransform == null) Initialize();
                
                return _hierarchyTransform;
            }  
        }
        public Sprite Icon
        {
            get
            {
                if(!_renderer) Initialize();
                return _renderer.sprite;
            }
        }

        private void Initialize()
        {
            if (_initialized) throw new Exception(); 
            
            _renderer = GetComponent<SpriteRenderer>();
            _hierarchyTransform = new();
            
            _hierarchyTransform.PositionChanged += (_, newValue) => transform.position = newValue;
            _hierarchyTransform.LossyScaleChanged += (_, newValue) => transform.localScale = newValue;
            _hierarchyTransform.RotationChanged += (_, newValue) => transform.localRotation = newValue;
            
            _initialized = true;
        }

        public void AddPort(IVisualPort port) => _ports.Add(port);
        public void Dispose() => Destroy(gameObject);
    }
}