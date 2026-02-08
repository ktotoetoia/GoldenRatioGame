using System;
using System.Linq;
using IM.Graphs;
using IM.Modules;
using IM.Storages;
using IM.UI;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.Visuals.Graph
{
    public class ModuleGraphEditorView : MonoBehaviour, IModuleGraphEditorView
    {
        [SerializeField] private UIDocument _storageUiDocument;
        private ModuleGraphVisualObserver _visualObserver;
        private IConditionalCommandModuleGraph _graph;
        private IModuleEntity _entity;
        private IStorageVisual _storageVisual;
        private VisualElement _storageVisualElement;
        private IExtensibleModule _selectedModule;
        private IModuleVisual _moduleVisual;
        private IModuleVisualObject _selectedModuleVisualObject;
        
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
            _storageVisualElement = _storageVisual as VisualElement;
            _storageVisualElement.visible = false;

            _storageVisual.ListView.RegisterCallback<MouseDownEvent>(evt =>
            {
                if (_storageVisual.ListView.selectedItem is not IStorageCell { Item: IExtensibleModule module } ||
                    !module.Extensions.TryGetExtension(out _moduleVisual)) return;
                
                _selectedModule = module;
                _selectedModuleVisualObject = _moduleVisual.EditorPool.Get();
                _selectedModuleVisualObject.Visible = true;
                _selectedModuleVisualObject.Transform.Transform.SetParent(transform,false);
                Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                position.Scale(new Vector3(1,1,0));

                _selectedModuleVisualObject.Transform.Position = position;
            });
        }

        private void Update()
        {
            if(_graph == null) return;

            if (Input.GetMouseButton(0) && _selectedModuleVisualObject != null)
            {
                Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                position.Scale(new Vector3(1,1,0));

                _selectedModuleVisualObject.Transform.Position = position;
            }

            if (Input.GetMouseButtonUp(0))
            {
                TryAddModule(_selectedModule);
                
                _moduleVisual.EditorPool.Release(_selectedModuleVisualObject);
                _moduleVisual = null;
                _selectedModule = null;
                _selectedModuleVisualObject = null;
            }
            
            GraphInput();
        }

        private void GraphInput()
        {
            if (Input.GetKeyDown(KeyCode.O)) AddModule();
            if (Input.GetKeyDown(KeyCode.P)) RemoveModule();
            if (Input.GetKeyDown(KeyCode.Z)) _graph.Undo(1);
            if (Input.GetKeyDown(KeyCode.X)) _graph.Redo(1);

            _visualObserver.OnGraphUpdated(_graph);
            _visualObserver.Update();
        }

        private void AddModule()
        {
            _entity.ModuleEditingContext.Storage.Select(x => x.Item).OfType<IExtensibleModule>().Any(TryAddModule);
        }

        private bool TryAddModule(IExtensibleModule module)
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
            IModule module = _entity.ModuleEditingContext.GraphEditor.Graph.Modules.LastOrDefault();

            if (_graph.CanRemoveModule(module))
            {
                _graph.RemoveModule(module);
            }
        }
        
        public void SetModuleEntity(IModuleEntity moduleEntity)
        {
            if(_entity != null) throw new Exception("Module entity has already been set");
            
            _entity = moduleEntity;
            _graph = _entity.ModuleEditingContext.GraphEditor.StartEditing();
            _visualObserver = new ModuleGraphVisualObserver(transform,false);
            _entity.Paused = true;
            
            _storageVisual?.SetStorage(moduleEntity.ModuleEditingContext.Storage);
            _storageVisualElement.visible = true;
        }

        public void ClearModuleEntity()
        {
            if(_entity == null) return;
            
            if(!_entity.ModuleEditingContext.GraphEditor.TrySaveChanges()) _entity.ModuleEditingContext.GraphEditor.CancelChanges();
            
            _visualObserver.Dispose();
            _visualObserver = null;
            _graph = null;
            _entity.Paused = false;
            _entity = null;
            _storageVisual?.ClearStorage();
            _storageVisualElement.visible = false;
        }
    }
}