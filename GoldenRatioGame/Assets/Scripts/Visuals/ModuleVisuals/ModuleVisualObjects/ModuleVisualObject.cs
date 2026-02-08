using System;
using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.Modules;
using IM.Transforms;
using UnityEngine;

namespace IM.Visuals
{
    [DisallowMultipleComponent]
    public class ModuleVisualObject : MonoBehaviour, IAnimatedModuleVisualObject
    {
        [SerializeField] private PortVisualObjectFactory _portVisualObjectFactory;
        [SerializeField] private PortBinderBase _portBinder;
        [SerializeField] private Renderer _renderer;
        private readonly List<IPortVisualObject> _portVisualObjects = new();
        private readonly List<IPoolObject> _poolObjects = new();
        private Animator _animator;
        private IExtensibleModule _owner;
        
        public ITransform Transform { get; private set; }
        public IReadOnlyList<IPortVisualObject> PortsVisualObjects => _portVisualObjects;
        public IPortVisualObjectDirtyTracker DirtyTracker { get; } = new PortVisualObjectDirtyTracker();

        public int Order
        {
            get => _renderer.sortingOrder;
            set => _renderer.sortingOrder = value;
        }

        public IExtensibleModule Owner
        {
            get => _owner;
            set
            {
                if(_owner != null) throw new InvalidOperationException("Owner can only be set once");
                
                _owner = value;
                _portVisualObjectFactory.CreateVisualObjects(_owner.Ports,_portVisualObjects,this);
                PortBinder = _portBinder;
            }
        }
        
        public Transform DefaultParent { get; set; }
        public bool IsAnimating { get; set; } = true;
        public IEnumerable<IAnimationChange> AnimationChanges { get; set; }

        public PortBinderBase PortBinder
        {
            get => _portBinder;
            set 
            { 
                _portBinder = value;
                _portBinder.Bind(_owner.Ports, _portVisualObjects);

                foreach (IPortVisualObject portVisualObject in _portVisualObjects) DirtyTracker.MarkDirty(portVisualObject);
            }
        }
        
        public bool Visible
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            Transform = GetComponent<ITransform>();
            GetComponents(_poolObjects);
            _poolObjects.Remove(this);
        }

        private void Update()
        {
            if (AnimationChanges == null || !IsAnimating || !Visible || !_animator || !_animator.isActiveAndEnabled ||
                !_animator.runtimeAnimatorController) return;
            
            foreach (IAnimationChange animationChange in AnimationChanges)
            {
                animationChange.ApplyToAnimator(_animator);
            }
        }

        public IPortVisualObject GetPortVisualObject(IPort port)
        {
            return _portVisualObjects.FirstOrDefault(x => x.Port == port);
        }

        public void OnInitializationFinished()
        {
            foreach (IRequireModuleVisualObjectInitialization initialization in
                     GetComponents<IRequireModuleVisualObjectInitialization>())
            {
                initialization.OnModuleVisualObjectInitialized(this);
            }
        }
        
        public void Dispose()
        {
            OnRelease();
            Destroy(gameObject);
        }

        public void OnRelease()
        {
            Visible = false;
            transform.SetParent(DefaultParent);
            Transform.LocalPosition = new Vector3(0, 0, 0);
            Transform.LocalScale = new Vector3(1, 1, 1);
            Transform.LocalRotation = Quaternion.identity;
            Order = 0;
            
            foreach (IPoolObject poolObject in _poolObjects) poolObject.OnRelease();
            foreach (IPortVisualObject portVisualObject in _portVisualObjects) portVisualObject.Reset();
        }

        public void OnGet()
        {
            Visible = true;
            
            foreach (IPoolObject poolObject in _poolObjects) poolObject.OnGet();
        }
    }
}