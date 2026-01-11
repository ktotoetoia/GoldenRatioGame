using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.Transforms;
using UnityEngine;

namespace IM.Visuals
{
    public class AnimationModule : MonoBehaviour,IAnimationModule
    {
        [SerializeField] private bool _isUsing;
        [SerializeField] private int _portNumber;
        [SerializeField] Vector2 _position;
        [SerializeField] float _rotation;
        
        private readonly List<ITransformPort> _ports = new();
        private readonly HierarchyTransform _hierarchyTransform = new();
        private Animator _animator;
        
        public Animator Animator => _animator??= GetComponent<Animator>();
        public IEnumerable<IEdge> Edges => _ports.Where(x => x.IsConnected).Select(x => x.Connection);
        public IEnumerable<ITransformPort> Ports => _ports;
        IEnumerable<IPort> IModule.Ports => _ports;
        public IHierarchyTransform HierarchyTransform => _hierarchyTransform;

        private void Awake()
        {
            _hierarchyTransform.PositionChanged += (_, newValue) => transform.position = newValue;
            _hierarchyTransform.LossyScaleChanged += (_, newValue) => transform.localScale = newValue;
            _hierarchyTransform.RotationChanged += (_, newValue) => transform.rotation = newValue;
        }
        
        public void AddPort(ITransformPort port) => _ports.Add(port);
        public void Dispose() => Destroy(gameObject);
        public IPortTransformChanger TransformChanger { get; set; }

        private void Update()
        {
            if(!_isUsing) return;
            
            TransformChanger?.SetPortPosition(_ports[_portNumber],_position);
            TransformChanger?.SetPortRotation(_ports[_portNumber],_rotation);
        }
    }
}