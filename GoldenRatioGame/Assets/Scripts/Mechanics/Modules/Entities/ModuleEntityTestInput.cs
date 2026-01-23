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
        [SerializeField] private ModuleEntity _moduleEntity;
        [SerializeField] private List<GameObject> _modulesPrefabs;
        private PreferredKeyboardBindingsAbilityUser _abilityUser;
        private IConditionalCommandModuleGraph _graph;
        private int _modulesAdded;
        public CellFactoryStorage Storage { get; set; } = new();

        private void Awake()
        {
            _abilityUser = new PreferredKeyboardBindingsAbilityUser(_moduleEntity.AbilityPool);

            foreach (GameObject prefab in _modulesPrefabs)
            {
                GameObject created = Instantiate(prefab);
                
                if (created.TryGetComponent(out ICoreExtensibleModule module))
                {
                    _moduleEntity.Initialize(module);
                    continue;
                }
                
                
                IStorageCell cell = Storage.FirstOrDefault(x => x.Item == null) ??
                                    Storage.CreateCellAt(Storage.Count);
                Storage.SetItem(cell, module);
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
                Debug.Log("start editing");
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if(_moduleEntity.GraphEditor.TrySaveChanges())
                {
                    Debug.Log("Edit Saved");
                    _graph = null;
                    return;
                }
                
                Debug.Log( "Save failed");
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                _moduleEntity.GraphEditor.CancelChanges();
                _graph = null;
                Debug.Log("Changes Cancelled");
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
            foreach (GameObject prefab in _modulesPrefabs)
            {
                GameObject created = Instantiate(prefab);
                IExtensibleModule module = created.GetComponent<IExtensibleModule>();

                foreach (IPort port in module.Ports)
                {
                    foreach (IPort targetPort in _graph.Modules.SelectMany(x => x.Ports))
                    {
                        if (_graph.CanAddAndConnect(module, port, targetPort))
                        {
                            IStorageCell cell = Storage.FirstOrDefault(x => x.Item == null) ??
                                                Storage.CreateCellAt(Storage.Count);
                            _graph.AddAndConnect(module, port, targetPort);

                            Storage.SetItem(cell, module);

                            return;
                        }
                    }
                }


                Destroy(created);
            }
        }

        private void RemoveModule()
        {     
            _graph.RemoveModule(_moduleEntity.GraphEditor.Graph.Modules.LastOrDefault(x => 
                x != _moduleEntity.GraphEditor.Graph.Modules.FirstOrDefault()));
        }
    }
}