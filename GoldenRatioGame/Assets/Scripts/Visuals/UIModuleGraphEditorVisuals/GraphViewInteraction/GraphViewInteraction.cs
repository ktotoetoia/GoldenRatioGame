using System;
using System.Linq;
using IM.Graphs;
using IM.LifeCycle;
using IM.Modules;
using IM.Visuals;
using IM.WeaponSystem;
using UnityEngine;

namespace IM.UI
{
    public class GraphViewInteraction : ContextViewer, IGraphViewInteraction,IPausable
    {
        [SerializeField] private ModuleGraphViewer moduleGraphViewer;
        [SerializeField] private StorageView _moduleStorageView;
        [SerializeField] private ModulePreviewPlacer _modulePreviewPlacer;
        [SerializeField] private WeaponPreviewPlacer _weaponPreviewPlacer;
        [SerializeField] private CurrentItemsViewer currentItemsViewer;
        [SerializeField] private Camera _uiCamera;
        [SerializeField] private float _addWorldDistance = 0.3f;
        private IModuleEditingContext _context;
        private IWeaponEditingService _weaponEditingService;
        
        public Func<Vector3> GetPointerPosition { get; set; } = ()  => Vector3.zero;
        public Func<bool> ShouldTryQuickRemoveAtPointer { get; set; } =() => false;
        public Func<bool> ShouldTryQuickRemove { get; set; } = () => false;
        public Func<bool> ShouldUndo { get; set; } = () => false;
        public Func<bool> ShouldRedo { get; set; } = () => false;
        public bool Paused { get; set; }
        
        private void Awake()
        {
            _moduleStorageView.StorageElement.ObjectInteracted += ObjectInteracted;
            _moduleStorageView.StorageElement.ObjectHovered += OnHold;
            _moduleStorageView.StorageElement.ObjectSelected += OnSelected;
            _moduleStorageView.StorageElement.ObjectReleased += OnRelease;

            _modulePreviewPlacer.HoverPositionSource = x => GetPointerPosition();
            _weaponPreviewPlacer.HoverPositionSource = x => GetPointerPosition();
        }

        private void Update()
        {
            if(_context != null && !Paused) GraphInput();
        }

        private void GraphInput()
        {
            if (ShouldTryQuickRemove()) _context.GraphEditing.TryQuickRemoveModule();
            if (!ShouldTryQuickRemoveAtPointer()) return;
            
            Vector3 mousePosition = GetPointerPosition();

            foreach (var (module, visual) in moduleGraphViewer.VisualObserver.ModuleToVisualObjects)
            {
                if (visual.Bounds.Contains(mousePosition) && _context.GraphEditing.TryQuickRemoveModule(module))
                {
                    break;
                }
            }
        }
        
        private void OnSelected(object obj)
        {
            if (!_weaponPreviewPlacer.IsPreviewing && obj is IWeapon weapon && _context!=null)
            {
                _weaponPreviewPlacer.StartPreview(weapon);

                return;
            }
            
            if (_modulePreviewPlacer.IsPreviewing || obj is not IExtensibleItem extensibleItem || _context == null) return;
            
            _modulePreviewPlacer.StartPreview(extensibleItem);
        }
        
        private void OnHold(object obj)
        {
            if(_modulePreviewPlacer.IsPreviewing&& _context != null) _modulePreviewPlacer.UpdatePreviewPosition();
            if(_weaponPreviewPlacer.IsPreviewing&& _context != null) _weaponPreviewPlacer.UpdatePreviewPosition();
        }

        private void OnRelease(object obj)
        {
            if (_modulePreviewPlacer.IsPreviewing)
            {
                if(_context!=null) TryAdd(_modulePreviewPlacer.PreviewVisual);
                _modulePreviewPlacer.FinalizePreview();
            }

            if (_weaponPreviewPlacer.IsPreviewing && _context != null )
            {
                if(_context!=null&&currentItemsViewer.GetContainerAt(GetPointerPosition()) is IWeaponContainer c ) _weaponEditingService.SetWeapon(c,_weaponPreviewPlacer.PreviewObject);
                _weaponPreviewPlacer.FinalizePreview();
            }
        }

        private void TryAdd(IModuleVisualObject toAdd)
        {
            IDataPort<IExtensibleItem> toAddPort = null;
            IDataPort<IExtensibleItem> onGraphPort = null;
            float distance = float.MaxValue;
            
            if(toAdd.Owner is ICoreExtensibleItem coreItem && _context.GraphEditing.TryQuickAddModule(_context.GraphEditing.CreateModule(coreItem))) return;

            foreach (IPortVisualObject a in toAdd.PortsVisualObjects)
            {
                foreach (IPortVisualObject otherPort in moduleGraphViewer.VisualObserver.ModuleToVisualObjects.SelectMany(x => x.Value.PortsVisualObjects))
                {
                    float newDistance = Vector3.Distance(otherPort.Transform.Position, a.Transform.Position);
                    IExtensibleItem item = toAdd.Owner;
                    
                    var module = _context.GraphEditing.CreateModule(item);
                    var modulePort = module.DataPorts.ElementAtOrDefault(toAdd.PortsVisualObjects.ToList().IndexOf(a));
                    var targetPort = moduleGraphViewer.VisualObserver.PortToVisualObjects.FirstOrDefault(x => x.Value == otherPort).Key;
                    
                    if (newDistance < distance && _context.GraphEditing.CanAddAndConnect(module, modulePort, targetPort))
                    {
                        distance = newDistance;
                        toAddPort = modulePort;
                        onGraphPort =  targetPort;
                    }
                }
            }
            
            if (distance > _addWorldDistance || toAddPort == null) return;
            
            _context.GraphEditing.AddAndConnect(toAddPort.DataModule,toAddPort,onGraphPort);
        }

        private void ObjectInteracted(object obj)
        {
            if(obj is IExtensibleItem item) _context.GraphEditing.TryQuickAddModule(_context.GraphEditing.CreateModule(item));
        }
        
        public override void SetContext(IModuleEditingContext moduleEditingContext)
        {
            _context = moduleEditingContext;
            _weaponEditingService = moduleEditingContext.Services.Get<IWeaponEditingService>();
        }
        
        public override void ClearContext()
        {
            _context = null;
            _weaponEditingService = null;
        }

        private void OnDestroy()
        {
            if (_moduleStorageView?.StorageElement == null) return;
            
            _moduleStorageView.StorageElement.ObjectInteracted -= ObjectInteracted;
            _moduleStorageView.StorageElement.ObjectHovered -= OnHold;
            _moduleStorageView.StorageElement.ObjectSelected -= OnSelected;
            _moduleStorageView.StorageElement.ObjectReleased -= OnRelease;
        }
    }
}