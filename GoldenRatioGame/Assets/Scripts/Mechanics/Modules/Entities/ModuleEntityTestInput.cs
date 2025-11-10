using System.Collections.Generic;
using System.Linq;
using IM.Abilities;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    [DefaultExecutionOrder(10000)]
    public class ModuleEntityTestInput : MonoBehaviour
    {
        [SerializeField] private ModuleEntity _moduleEntity;
        [SerializeField] private List<GameObject> _modulesPrefabs;
        private int _modulesAdded;
        private PreferredKeyboardBindingsAbilityUser _abilityUser;
        private IConditionalCommandModuleGraph  _graph;
        
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
            
            if (Input.GetKeyDown(KeyCode.O))
            {
                IModule module = GetNextModule();
                
                if(module == null) return;
                
                _graph.AddAndConnect(module,
                    module.Ports.FirstOrDefault(x => !x.IsConnected),
                    _graph.Modules.FirstOrDefault(x => x.Ports.Any(p => !p.IsConnected))?.Ports
                        .FirstOrDefault(x => !x.IsConnected));
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

        private IModule GetNextModule()
        {
            if(_modulesPrefabs.Count < _modulesAdded) return null;
            
            IModule module = Instantiate(_modulesPrefabs[_modulesAdded]).GetComponent<IModule>();
            
            _modulesAdded++;
            
            return module;
        }
    }
}