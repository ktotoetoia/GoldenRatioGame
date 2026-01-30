using System.Linq;
using IM.Graphs;
using IM.Modules;
using UnityEngine;

namespace IM.Visuals.Graph
{
    public class ModuleGraphEditorView : MonoBehaviour, IModuleGraphEditorView
    {
        private ModuleGraphVisualObserver _visualObserver;
        private IConditionalCommandModuleGraph _graph;
        private IModuleEntity _entity;
        
        public void SetModuleEntity(IModuleEntity moduleEntity)
        {
            if(_entity != null) throw new System.Exception("Module entity has already been set");
            
            _entity = moduleEntity;
            _graph = _entity.ModuleEditingContext.GraphEditor.StartEditing();
            _visualObserver = new ModuleGraphVisualObserver();
            _entity.Paused = true;
        }

        public void ClearModuleEntity()
        {
            if(!_entity.ModuleEditingContext.GraphEditor.TrySaveChanges()) _entity.ModuleEditingContext.GraphEditor.CancelChanges();
            
            _visualObserver.Dispose();
            _visualObserver = null;
            _graph = null;
            _entity.Paused = false;
            _entity = null;
        }

        private void Update()
        {
            if(_graph == null) return;
            
            GraphInput();
        }

        private void GraphInput()
        {
            if (Input.GetKeyDown(KeyCode.O)) AddModule();
            if (Input.GetKeyDown(KeyCode.P)) RemoveModule();
            if (Input.GetKeyDown(KeyCode.Z)) _graph.Undo(1);
            if (Input.GetKeyDown(KeyCode.X)) _graph.Redo(1);
            
            _visualObserver.OnGraphUpdated(_graph);
        }

        private void AddModule()
        {
            if (_graph.Modules.Count == 0)
            {
                foreach (ICoreExtensibleModule module in _entity.ModuleEditingContext.Storage.Select(x => x.Item).OfType<ICoreExtensibleModule>())
                {
                    if (_graph.CanAddModule(module))
                    {
                        _graph.AddModule(module);

                        return;
                    }
                }
            }
            
            foreach (IExtensibleModule module in _entity.ModuleEditingContext.Storage.Select(x => x.Item).OfType<IExtensibleModule>())
            {
                foreach (IPort port in module.Ports)
                {
                    foreach (IPort targetPort in _graph.Modules.SelectMany(x => x.Ports))
                    {
                        if (!_graph.CanAddAndConnect(module, port, targetPort)) continue;
                        
                        _graph.AddAndConnect(module, port, targetPort);
                            
                        return;
                    }
                }
            }
        }

        private void RemoveModule()
        {
            IModule module = _entity.ModuleEditingContext.GraphEditor.Graph.Modules.LastOrDefault();
            
            _graph.RemoveModule(module);
        }
    }
}