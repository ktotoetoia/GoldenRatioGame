using System;
using System.Linq;
using IM.Graphs;
using IM.Modules;
using IM.Visuals;
using UnityEngine;

namespace IM.UI
{
    public class GraphViewInteraction : ContextVisualizer, IGraphViewInteraction
    {
        [SerializeField] private ModuleGraphView _moduleGraphView;
        [SerializeField] private StorageView _storageView;
        [SerializeField] private ModulePreviewPlacerMono _previewPlacer;
        [SerializeField] private Camera _uiCamera;
        [SerializeField] private float _addWorldDistance = 0.3f;
        private IModuleEditingContext _moduleEditingContext;
        private IGraphOperations<IExtensibleItem> _graphOperations;

        public Func<Vector3> GetPointerPosition { get; set; } = ()  => Vector3.zero;
        public Func<bool> ShouldTryQuickRemoveAtPointer { get; set; } =() => false;
        public Func<bool> ShouldTryQuickRemove { get; set; } = () => false;
        public Func<bool> ShouldUndo { get; set; } = () => false;
        public Func<bool> ShouldRedo { get; set; } = () => false;
        
        private void Awake()
        {
            _storageView.StorageElement.ObjectInteracted += ObjectInteracted;
            _storageView.StorageElement.ObjectHovered += OnHold;
            _storageView.StorageElement.ObjectSelected += OnSelected;
            _storageView.StorageElement.ObjectReleased += OnRelease;

            _previewPlacer.HoverPositionSource = x => GetPointerPosition();
        }

        private void Update()
        {
            if(_graphOperations != null) GraphInput();
        }

        private void GraphInput()
        {
            if (ShouldTryQuickRemove()) _graphOperations.TryQuickRemoveModule();
            if (ShouldUndo()) _graphOperations.Undo(1);
            if (ShouldRedo()) _graphOperations.Redo(1);
            
            if (ShouldTryQuickRemoveAtPointer())
            {
                Vector3 mousePosition = GetPointerPosition();

                foreach (var x in _moduleGraphView.VisualObserver.ModuleToVisualObjects)
                {
                    if (x.Value.Bounds.Contains(mousePosition) && _graphOperations.TryQuickRemoveModule(x.Key))
                    {
                        break;
                    }
                }
            }
        }
        
        private void OnSelected(object obj)
        {
            if (_previewPlacer.IsPreviewing || obj is not IExtensibleItem module || _graphOperations == null) return;
            
            _previewPlacer.StartPreview(module);
        }
        
        private void OnHold(object obj)
        {
            if(_previewPlacer.IsPreviewing&& _graphOperations != null) _previewPlacer.UpdatePreviewPosition();
        }

        private void OnRelease(object obj)
        {
            if (_previewPlacer.IsPreviewing && _graphOperations != null)
            {
                TryAdd(_previewPlacer.PreviewObject);
                _previewPlacer.FinalizePreview();
            }
        }

        private void TryAdd(IModuleVisualObject toAdd)
        {
            IDataPort<IExtensibleItem> toAddPort = null;
            IDataPort<IExtensibleItem> onGraphPort = null;
            float distance = float.MaxValue;
            
            if(toAdd.Owner is ICoreExtensibleItem c && _graphOperations.TryQuickAddModule(_moduleEditingContext.CreateModule(c))) return; 

            foreach (IPortVisualObject a in toAdd.PortsVisualObjects)
            {
                foreach (IPortVisualObject otherPort in _moduleGraphView.VisualObserver.ModuleToVisualObjects.SelectMany(x => x.Value.PortsVisualObjects))
                {
                    float newDistance = Vector3.Distance(otherPort.Transform.Position, a.Transform.Position);
                    IExtensibleItem item = toAdd.Owner;
                    var gh = _moduleEditingContext.CreateModule(item);
                    
                    if (newDistance < distance && _graphOperations.Graph
                            .CanAddAndConnect(gh, gh.DataPorts.ElementAtOrDefault(toAdd.PortsVisualObjects.ToList().IndexOf(a)),
                                _moduleGraphView.VisualObserver.PortToVisualObjects.FirstOrDefault(x => x.Value == otherPort).Key))
                    {
                        distance = newDistance;
                        toAddPort = gh.DataPorts.ElementAtOrDefault(toAdd.PortsVisualObjects.ToList().IndexOf(a));
                        onGraphPort =  _moduleGraphView.VisualObserver.PortToVisualObjects.FirstOrDefault(x => x.Value == otherPort).Key;
                    }
                }
            }
            
            if (distance > _addWorldDistance || toAddPort == null) return;
            _graphOperations.Graph.AddAndConnect(toAddPort.DataModule,toAddPort,onGraphPort);
        }

        private void ObjectInteracted(object obj)
        {
            if(obj is IExtensibleItem item) _graphOperations?.TryQuickAddModule(_moduleEditingContext.CreateModule(item));
        }
        
        public override void SetContext(IModuleEditingContext moduleEditingContext)
        {
            _moduleEditingContext = moduleEditingContext;
            _graphOperations = new CommandGraphOperations<IExtensibleItem>(moduleEditingContext.ModuleGraph);
        }
        
        public override void ClearContext()
        {
            _graphOperations = null;
        }

        private void OnDestroy()
        {
            if (_storageView?.StorageElement == null) return;
            
            _storageView.StorageElement.ObjectInteracted -= ObjectInteracted;
            _storageView.StorageElement.ObjectHovered -= OnHold;
            _storageView.StorageElement.ObjectSelected -= OnSelected;
            _storageView.StorageElement.ObjectReleased -= OnRelease;
        }
    }
}