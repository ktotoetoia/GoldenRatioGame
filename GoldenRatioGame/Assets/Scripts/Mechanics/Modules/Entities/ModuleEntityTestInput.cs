using System;
using System.Collections.Generic;
using System.Linq;
using IM.Abilities;
using IM.Graphs;
using IM.Storages;
using UnityEngine;

namespace IM.Modules
{
    [DefaultExecutionOrder(10000)]
    public class ModuleEntityTestInput : MonoBehaviour
    {
        [SerializeField] private bool _debugEditor;
        [SerializeField] private ModuleEntity _moduleEntity;
        [SerializeField] private List<GameObject> _modulesPrefabs;
        private readonly ModuleTransitioner _moduleTransitioner = new();
        private PreferredKeyboardBindingsAbilityUser _abilityUser;
        private IConditionalCommandModuleGraph _graph;
        private int _modulesAdded;
        public CellFactoryStorage Storage { get; } = new();

        private void Awake()
        {
            _abilityUser = new PreferredKeyboardBindingsAbilityUser(_moduleEntity.AbilityPool);

            foreach (GameObject prefab in _modulesPrefabs)
            {
                GameObject created = Instantiate(prefab);
                IExtensibleModule module = created.GetComponent<IExtensibleModule>();
                
                if (module is ICoreExtensibleModule)
                {
                    _moduleEntity.Initialize(module);
                    continue;
                }
                
                _moduleTransitioner.ClearState(module,Storage,_graph);
                _moduleTransitioner.SetStateToStorage(module,Storage,Storage.FirstOrDefault(x => x.Item == null) ??
                                                                     Storage.CreateCell());
            }
        }

        private void Update()
        {
            _abilityUser.Update();
            EditorInput();
            GraphInput();
        }

        private void EditorInput()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                _graph = _moduleEntity.GraphEditor.StartEditing();
                if(_debugEditor) Debug.Log("start editing");
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if(_moduleEntity.GraphEditor.TrySaveChanges())
                {
                    if(_debugEditor) Debug.Log("Edit Saved");
                    _graph = null;
                    return;
                }
                
                if(_debugEditor) Debug.Log( "Save failed");
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                _moduleEntity.GraphEditor.CancelChanges();
                _graph = null;
                if(_debugEditor) Debug.Log("Changes Cancelled");
            }
        }

        private void GraphInput()
        {
            if(_graph == null) return;
            
            if (Input.GetKeyDown(KeyCode.O) && _moduleEntity.GraphEditor.IsEditing) AddModule();
            if (Input.GetKeyDown(KeyCode.P)) RemoveModule();
            if (Input.GetKeyDown(KeyCode.Z)) _graph.Undo(1);
            if (Input.GetKeyDown(KeyCode.X)) _graph.Redo(1);
        }

        private void AddModule()
        {
            foreach (IExtensibleModule module in Storage.Select(x => x.Item).Where(x => x is IExtensibleModule))
            {
                foreach (IPort port in module.Ports)
                {
                    foreach (IPort targetPort in _graph.Modules.SelectMany(x => x.Ports))
                    {
                        if (!_graph.CanAddAndConnect(module, port, targetPort)) continue;
                        
                        _moduleTransitioner.ClearState(module,Storage, _graph);
                        _moduleTransitioner.SetStateToGraphAndAdd(module,port,targetPort,_graph);
                            
                        return;
                    }
                }
            }
        }

        private void RemoveModule()
        {
            IModule module = _moduleEntity.GraphEditor.Graph.Modules.LastOrDefault(x =>
                x != _moduleEntity.GraphEditor.Graph.Modules.FirstOrDefault());
            
            _moduleTransitioner.ClearState(module as IExtensibleModule, Storage,_graph);
            _moduleTransitioner.SetStateToStorage(module as IExtensibleModule, Storage,Storage.FirstOrDefault(x => x.Item == null));
        }
    }
}