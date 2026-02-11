using System;
using System.Linq;
using IM.Graphs;
using IM.Modules;
using IM.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.Visuals.Graph
{
    public class ModuleContextView : MonoBehaviour, IModuleContextView
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _toAdd;
        [SerializeField] private UIDocument _storageUiDocument;
        [SerializeField] private ModuleVisualObjectPreset _preset;
        private ModuleGraphVisualObserver _visualObserver;
        private IModulePreviewPlacer _previewPlacer;
        private IConditionalCommandModuleGraph _graph;
        private IModuleEditingContext _moduleEditingContext;
        private IStorageVisual _storageVisual;
        
        private void Awake()
        {
            if (!_storageUiDocument ||
                _storageUiDocument.rootVisualElement.Query().ToList().FirstOrDefault(x => x is IStorageVisual) is not 
                    IStorageVisual storageVisual)
            {
                Debug.LogWarning("Storage visual was not found, only editor would be shown");
                return;
            }
            
            _storageVisual = storageVisual;
            
            _previewPlacer = new ModulePreviewPlacer(_camera, _toAdd, new ModuleVisualObjectPreset(1,5,true));
            _storageUiDocument.rootVisualElement.visible = false;

            _storageVisual.ObjectInteracted += ObjectInteracted;
            _storageVisual.ObjectHovered += OnHold;
            _storageVisual.ObjectSelected += OnSelected;
            _storageVisual.ObjectReleased += OnRelease;
        }

        private void Update()
        {
            if(_graph == null) return;
            
            GraphInput();
        }

        private void OnSelected(object obj)
        {
            if (_previewPlacer.IsPreviewing || obj is not IExtensibleModule module ) return;
            
            _previewPlacer.StartPreview(module);
        }
        
        private void OnHold(object obj)
        {
            if(_previewPlacer.IsPreviewing ) _previewPlacer.UpdatePreviewPosition();
        }

        private void OnRelease(object obj)
        {
            if (_previewPlacer.IsPreviewing) TryAddModule(_previewPlacer.FinalizePreview());
        }

        private void ObjectInteracted(object obj)
        {
            TryAddModule(obj as IModule);
        }

        private void GraphInput()
        {
            if (Input.GetKeyDown(KeyCode.P)) RemoveModule();
            if (Input.GetKeyDown(KeyCode.Z)) _graph.Undo(1);
            if (Input.GetKeyDown(KeyCode.X)) _graph.Redo(1);

            _visualObserver.OnGraphUpdated(_graph);
            _visualObserver.Update();
        }

        private bool TryAddModule(IModule module)
        {
            if (module is ICoreExtensibleModule)
            {
                if (_graph.CanAddModule(module))
                {
                    _graph.AddModule(module);

                    return true;
                }

                return false;
            }
            
            foreach (IPort port in module.Ports)
            {
                foreach (IPort targetPort in _graph.Modules.SelectMany(x => x.Ports))
                {
                    if (!_graph.CanAddAndConnect(module, port, targetPort)) continue;
                        
                    _graph.AddAndConnect(module, port, targetPort);
                            
                    return true;
                }
            }

            return false;
        }

        private void RemoveModule()
        {   
            IModule module = _moduleEditingContext.GraphEditor.Graph.Modules.LastOrDefault();

            if (!_graph.CanRemoveModule(module)) return;
            
            _graph.RemoveModule(module);
        }
        
        public void SetModuleContext(IModuleEditingContext moduleEntity)
        {
            if(_moduleEditingContext != null) throw new Exception("Module entity has already been set");
            
            _moduleEditingContext = moduleEntity;
            _graph = _moduleEditingContext.GraphEditor.StartEditing();
            _visualObserver = new ModuleGraphVisualObserver(_toAdd,false,_preset);
            _storageVisual?.SetStorage(_moduleEditingContext.Storage);
            _storageUiDocument.rootVisualElement.visible = true;
        }

        public void ClearModuleContext()
        {
            if(_moduleEditingContext == null) return;
            
            if(!_moduleEditingContext.GraphEditor.TrySaveChanges()) _moduleEditingContext.GraphEditor.CancelChanges();
            
            _visualObserver.Dispose();
            _visualObserver = null;
            _graph = null;
            _moduleEditingContext = null;
            _storageVisual?.ClearStorage();
            _storageUiDocument.rootVisualElement.visible = false;
        }

        private void OnDestroy()
        {
            _storageVisual.ObjectInteracted -= ObjectInteracted;
            _storageVisual.ObjectHovered -= OnHold;
            _storageVisual.ObjectSelected -= OnSelected;
            _storageVisual.ObjectReleased -= OnRelease;
        }
    }
}