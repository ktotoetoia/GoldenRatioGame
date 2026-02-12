using IM.Graphs;
using IM.Modules;
using UnityEngine;

namespace IM.Visuals.Graph
{
    public class ModuleContextInput : MonoBehaviour, IModuleContextInput
    {
        [SerializeField] private ModuleGraphView _moduleGraphView;
        [SerializeField] private StorageView _storageView;
        [SerializeField] private ModulePreviewPlacerMono _previewPlacer;
        private IGraphOperations _graphOperations;
        
        private void Awake()
        {
            _storageView.StorageVisual.ObjectInteracted += ObjectInteracted;
            _storageView.StorageVisual.ObjectHovered += OnHold;
            _storageView.StorageVisual.ObjectSelected += OnSelected;
            _storageView.StorageVisual.ObjectReleased += OnRelease;
        }

        private void Update()
        {
            if(_graphOperations != null) GraphInput();
        }

        private void GraphInput()
        {
            if (Input.GetKeyDown(KeyCode.P)) _graphOperations.QuickRemoveModule();
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
            if (_previewPlacer.IsPreviewing && _graphOperations != null) _graphOperations.QuickAddModule(_previewPlacer.FinalizePreview());
        }

        private void ObjectInteracted(object obj)
        {
            _graphOperations?.QuickAddModule(obj as IModule);
        }
        
        public void SetGraph(IConditionalCommandModuleGraph graph)
        {
            _graphOperations = new CommandGraphOperations(graph);
        }

        public void ClearGraph()
        {
            _graphOperations = null;
        }

        private void OnDestroy()
        {
            if (_storageView?.StorageVisual == null) return;
            
            _storageView.StorageVisual.ObjectInteracted -= ObjectInteracted;
            _storageView.StorageVisual.ObjectHovered -= OnHold;
            _storageView.StorageVisual.ObjectSelected -= OnSelected;
            _storageView.StorageVisual.ObjectReleased -= OnRelease;
        }
    }
}