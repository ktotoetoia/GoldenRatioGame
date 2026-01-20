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
        private IConditionalCommandModuleGraph  _graph;
        private int _modulesAdded;
        public CellFactoryStorage Storage { get; set; } = new CellFactoryStorage();

        private void Awake()
        {
            _abilityUser = new PreferredKeyboardBindingsAbilityUser(_moduleEntity.AbilityPool);
        }

        private void Update()
        {
            _abilityUser.Update();
            
            if (Input.GetKeyDown(KeyCode.Q))
            {
                _graph = _moduleEntity.GraphEditor.StartEditing();
                Debug.Log("start editing");
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log(_moduleEntity.GraphEditor.TrySaveChanges() ? "Edit saved" : "Save failed");
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                _moduleEntity.GraphEditor.CancelChanges();
                Debug.Log("Changes Cancelled");
            }
            
            if (Input.GetKeyDown(KeyCode.O) && _moduleEntity.GraphEditor.IsEditing)
            {
                foreach (GameObject prefab in _modulesPrefabs)
                {
                    GameObject created =  Instantiate(prefab);
                    IExtensibleModule module = created.GetComponent<IExtensibleModule>();
                    
                    foreach (IPort port in module.Ports)
                    {
                        foreach (IPort targetPort in _graph.Modules.SelectMany(x => x.Ports))
                        {
                            if (_graph.CanAddAndConnect(module, port, targetPort))
                            {
                                IStorageCell cell  = Storage.FirstOrDefault(x => x.Item==null) ?? Storage.CreateCellAt(Storage.Count);
                                _graph.AddAndConnect(module,port, targetPort);
                                
                                Storage.SetItem(cell,module);
                                
                                return;
                            }
                        }
                    }

                    
                    Destroy(created);
                }
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                _graph.RemoveModule(_moduleEntity.GraphEditor.Graph.Modules.LastOrDefault(x => x != _moduleEntity.GraphEditor.Graph.Modules.FirstOrDefault()));
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                _graph.Undo(1);
            }
            
            if (Input.GetKeyDown(KeyCode.X))
            {
                _graph.Redo(1);
            }
        }
    }
}