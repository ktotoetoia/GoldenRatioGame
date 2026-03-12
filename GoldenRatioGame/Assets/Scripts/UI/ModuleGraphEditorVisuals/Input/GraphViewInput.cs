using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.Modules;
using UnityEngine;

namespace IM.Visuals.Graph
{
    public class GraphViewInput : MonoBehaviour, IGraphViewInput
    {
        [SerializeField] private ModuleGraphView _moduleGraphView;
        [SerializeField] private StorageView _storageView;
        [SerializeField] private ModulePreviewPlacerMono _previewPlacer;
        [SerializeField] private Camera _uiCamera;
        [SerializeField] private float _addWorldDistance = 0.3f;
        private IGraphOperations _graphOperations;
        
        private void Awake()
        {
            _storageView.StorageElement.ObjectInteracted += ObjectInteracted;
            _storageView.StorageElement.ObjectHovered += OnHold;
            _storageView.StorageElement.ObjectSelected += OnSelected;
            _storageView.StorageElement.ObjectReleased += OnRelease;

            _previewPlacer.HoverPositionSource = x => GetMousePosition();
        }

        private void Update()
        {
            if(_graphOperations != null) GraphInput();
        }

        private void GraphInput()
        {
            if (Input.GetMouseButtonDown(1))
            {
                Vector2 mousePosition = GetMousePosition();

                foreach (KeyValuePair<IExtensibleModule, IModuleVisualObject> pair in _moduleGraphView.VisualObserver.ModuleToVisualObjects.Where(x => x.Value is IEditorModuleVisualObject e && e.EditorBounds.Contains(mousePosition)))
                {
                    if(_graphOperations.TryQuickRemoveModule(pair.Key)) return;
                }
            }
            
            if (Input.GetKeyDown(KeyCode.P)) _graphOperations.TryQuickRemoveModule();
            if (Input.GetKeyDown(KeyCode.Z)) _graphOperations.Undo(1);
            if (Input.GetKeyDown(KeyCode.X)) _graphOperations.Redo(1);
        }
        
        private void OnSelected(object obj)
        {
            if (_previewPlacer.IsPreviewing || obj is not IExtensibleModule module || _graphOperations == null) return;
            
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
            IPortVisualObject toAddPort = null;
            IPortVisualObject onGraphPort = null;
            float distance = float.MaxValue;
            
            if(toAdd.Owner is ICoreExtensibleModule c && _graphOperations.TryQuickAddModule(c)) return; 

            foreach (IPortVisualObject a in toAdd.PortsVisualObjects)
            {
                foreach (IPortVisualObject otherPort in _moduleGraphView.VisualObserver.ModuleToVisualObjects.SelectMany(x => x.Value.PortsVisualObjects))
                {
                    float newDistance = Vector3.Distance(otherPort.Transform.Position, a.Transform.Position);
                    
                    if (newDistance < distance && _graphOperations.Graph.CanAddAndConnect(toAdd.Owner, a.Port,otherPort.Port))
                    {
                        distance = newDistance;
                        toAddPort = a;
                        onGraphPort = otherPort;
                    }
                }
            }
            
            if (distance > _addWorldDistance || toAddPort == null) return;
            _graphOperations.Graph.AddAndConnect(toAdd.Owner,toAddPort.Port,onGraphPort.Port);
        }

        private void ObjectInteracted(object obj)
        {
            _graphOperations?.TryQuickAddModule(obj as IModule);
        }
        
        public void SetGraph(IConditionalCommandModuleGraph graph)
        {
            _graphOperations = new CommandGraphOperations(graph);
        }

        public void ClearGraph()
        {
            _graphOperations = null;
        }

        private Vector3 GetMousePosition()
        {
            return (Vector2)_uiCamera.ScreenToWorldPoint(Input.mousePosition);
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