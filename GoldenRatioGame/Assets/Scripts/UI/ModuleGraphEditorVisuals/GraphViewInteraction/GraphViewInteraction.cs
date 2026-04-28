using System;
using System.Linq;
using IM.Graphs;
using IM.Modules;
using IM.Visuals;
using UnityEngine;

namespace IM.UI
{
    public class GraphViewInteraction : ContextViewer, IGraphViewInteraction
    {
        [SerializeField] private ModuleGraphView _moduleGraphView;
        [SerializeField] private StorageView _moduleStorageView;
        [SerializeField] private ModulePreviewPlacer _modulePreviewPlacer;
        [SerializeField] private Camera _uiCamera;
        [SerializeField] private float _addWorldDistance = 0.3f;
        private IModuleEditingContext _context;
        
        public Func<Vector3> GetPointerPosition { get; set; } = ()  => Vector3.zero;
        public Func<bool> ShouldTryQuickRemoveAtPointer { get; set; } =() => false;
        public Func<bool> ShouldTryQuickRemove { get; set; } = () => false;
        public Func<bool> ShouldUndo { get; set; } = () => false;
        public Func<bool> ShouldRedo { get; set; } = () => false;
        
        private void Awake()
        {
            _moduleStorageView.StorageElement.ObjectInteracted += ObjectInteracted;
            _moduleStorageView.StorageElement.ObjectHovered += OnHold;
            _moduleStorageView.StorageElement.ObjectSelected += OnSelected;
            _moduleStorageView.StorageElement.ObjectReleased += OnRelease;

            _modulePreviewPlacer.HoverPositionSource = x => GetPointerPosition();
        }

        private void Update()
        {
            if(_context != null) GraphInput();
        }

        private void GraphInput()
        {
            if (ShouldTryQuickRemove()) _context.GraphEditing.TryQuickRemoveModule();
            if (!ShouldTryQuickRemoveAtPointer()) return;
            
            Vector3 mousePosition = GetPointerPosition();

            foreach (var (module, visual) in _moduleGraphView.VisualObserver.ModuleToVisualObjects)
            {
                if (visual.Bounds.Contains(mousePosition) && _context.GraphEditing.TryQuickRemoveModule(module))
                {
                    break;
                }
            }
        }
        
        private void OnSelected(object obj)
        {
            if (_modulePreviewPlacer.IsPreviewing || obj is not IExtensibleItem extensibleItem || _context == null) return;
            
            _modulePreviewPlacer.StartPreview(extensibleItem);
        }
        
        private void OnHold(object obj)
        {
            if(_modulePreviewPlacer.IsPreviewing&& _context != null) _modulePreviewPlacer.UpdatePreviewPosition();
        }

        private void OnRelease(object obj)
        {
            if (_modulePreviewPlacer.IsPreviewing && _context != null)
            {
                TryAdd(_modulePreviewPlacer.PreviewObject);
                _modulePreviewPlacer.FinalizePreview();
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
                foreach (IPortVisualObject otherPort in _moduleGraphView.VisualObserver.ModuleToVisualObjects.SelectMany(x => x.Value.PortsVisualObjects))
                {
                    float newDistance = Vector3.Distance(otherPort.Transform.Position, a.Transform.Position);
                    IExtensibleItem item = toAdd.Owner;
                    
                    var module = _context.GraphEditing.CreateModule(item);
                    var modulePort = module.DataPorts.ElementAtOrDefault(toAdd.PortsVisualObjects.ToList().IndexOf(a));
                    var targetPort = _moduleGraphView.VisualObserver.PortToVisualObjects.FirstOrDefault(x => x.Value == otherPort).Key;
                    
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
        }
        
        public override void ClearContext()
        {
            _context = null;
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