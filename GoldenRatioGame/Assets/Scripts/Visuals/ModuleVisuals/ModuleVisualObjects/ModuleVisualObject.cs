using System;
using System.Collections.Generic;
using IM.LifeCycle;
using IM.Modules;
using IM.Transforms;
using UnityEngine;

namespace IM.Visuals
{
    public class ModuleVisualObject : MonoBehaviour, IModuleVisualObject, IParentRestorable
    {
        [SerializeField] protected PortVisualObjectFactoryBase _portVisualObjectFactory;
        [SerializeField] protected PortBinderBase _portBinder;
        [SerializeField] protected Renderer _renderer;
        [SerializeField] private BoundsSource _boundsSource = BoundsSource.Renderer;
        private readonly List<IPortVisualObject> _portVisualObjects = new();
        private readonly List<IPoolObject> _poolObjects = new();
        private readonly Bounds _defaultEditorLocalBounds = new (Vector3.zero, Vector3.one);
        private IVisualObjectPreset _preset;

        public Bounds LocalBounds
        {
            get
            {
                switch (_boundsSource)
                {
                    case BoundsSource.Renderer:
                        return _renderer.localBounds;
                    default:
                        return _defaultEditorLocalBounds;
                }
            }
        }

        public Bounds Bounds
        {
            get
            {
                switch (_boundsSource)
                {
                    case BoundsSource.Renderer:
                        return _renderer.bounds;
                    default:
                        return new(transform.position, _defaultEditorLocalBounds.size);
                }
            }
        }

        private ITransform _transform;
        public ITransform Transform => _transform ??= GetComponent<ITransform>();
        public IReadOnlyList<IPortVisualObject> PortsVisualObjects => _portVisualObjects;
        public IPortVisualObjectDirtyTracker DirtyTracker { get; } = new PortVisualObjectDirtyTracker();
        public IExtensibleItem Owner { get; private set; }
        public Transform DefaultParent { get; set; }

        public int Order
        {
            get => _renderer.sortingOrder;
            set 
            {
                _renderer.sortingOrder = value;
                
                foreach (IPortVisualObject portVisualObject in _portVisualObjects)
                {
                    portVisualObject.Order = value + 1;
                }
            }
        }

        public float ModuleLocalOrder
        {
            get => transform.position.z;
            set
            {
                var vector3 = transform.position;
                vector3.z = value;
                transform.position = vector3;
            }
        }

        public int Layer
        {
            get => gameObject.layer;
            set
            {
                if (gameObject.layer == value) return;

                SetLayerRecursively(transform, value);
            }
        }

        private void SetLayerRecursively(Transform targetTransform, int layer)
        {
            targetTransform.gameObject.layer = layer;

            for (int i = 0; i < targetTransform.childCount; i++)
            {
                SetLayerRecursively(targetTransform.GetChild(i), layer);
            }
        }

        public PortBinderBase PortBinder
        {
            get => _portBinder;
            set 
            { 
                _portBinder = value;
                _portBinder.Bind(_portVisualObjects);

                foreach (IPortVisualObject portVisualObject in _portVisualObjects) DirtyTracker.MarkDirty(portVisualObject);
            }
        }
        
        public bool Visible
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }

        public void FinishInitialization(IExtensibleItem owner)
        {
            if(Owner != null) throw new InvalidOperationException("Owner can only be set once");
            
            _preset = new ModuleVisualObjectPreset(Order, gameObject.layer)
            {
                PortsVisible = true
            };
            
            GetComponentsInChildren(_poolObjects);
            _poolObjects.Remove(this);
            
            Owner = owner;
            _portVisualObjectFactory.CreateVisualObjects(_portVisualObjects,this);
            PortBinder = _portBinder;
            
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
            ResetToDefaultParent();
            _preset.ApplyTo(this);
            
            foreach (IPoolObject poolObject in _poolObjects) poolObject.OnRelease();
            foreach (IPortVisualObject portVisualObject in _portVisualObjects) portVisualObject.Reset();
        }

        public void OnGet()
        {
            Visible = true;
            
            foreach (IPoolObject poolObject in _poolObjects) poolObject.OnGet();
        }

        public void ResetToDefaultParent()
        {
            transform.SetParent(DefaultParent);
        }
    }
}