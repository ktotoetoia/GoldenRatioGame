using System;
using System.Collections.Generic;
using System.Linq;
using IM.Base;
using IM.Graphs;
using IM.Modules;
using IM.Transforms;
using UnityEngine;

namespace IM.Visuals
{
    public class ModuleVisualObject : MonoBehaviour, IModuleVisualObject
    {
        [SerializeField] private PortVisualObjectFactoryBase _portVisualObjectFactory;
        [SerializeField] private PortBinderBase _portBinder;
        [SerializeField] private Renderer _renderer;
        private readonly List<IPortVisualObject> _portVisualObjects = new();
        private readonly List<IPoolObject> _poolObjects = new();
        private IModuleVisualObjectPreset _preset;
        
        public ITransform Transform { get; private set; }
        public IReadOnlyList<IPortVisualObject> PortsVisualObjects => _portVisualObjects;
        public IPortVisualObjectDirtyTracker DirtyTracker { get; } = new PortVisualObjectDirtyTracker();
        public IPaletteSwapper PaletteSwapper { get; private set; }
        public IExtensibleModule Owner { get; private set; }

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
                _portBinder.Bind(Owner.Ports, _portVisualObjects);

                foreach (IPortVisualObject portVisualObject in _portVisualObjects) DirtyTracker.MarkDirty(portVisualObject);
            }
        }
        
        public bool Visible
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }
        
        protected virtual void Awake()
        {
            _preset = new ModuleVisualObjectPreset(Order, gameObject.layer)
            {
                PortsVisible = true
            };
            
            PaletteSwapper = new PaletteSwapper(_renderer);
            Transform = GetComponent<ITransform>();
            GetComponentsInChildren(_poolObjects);
            _poolObjects.Remove(this);
        }

        public IPortVisualObject GetPortVisualObject(IPort port)
        {
            return _portVisualObjects.FirstOrDefault(x => x.Port == port);
        }

        public void FinishInitialization(IExtensibleModule owner)
        {
            if(Owner != null) throw new InvalidOperationException("Owner can only be set once");
            
            Owner = owner;
            _portVisualObjectFactory.CreateVisualObjects(Owner.Ports,_portVisualObjects,this);
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
            transform.SetParent(DefaultParent);
            _preset.ApplyTo(this);
            PaletteSwapper.AppliedPalette = PaletteSwapper.SourcePalette;
            
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