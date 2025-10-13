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
        [SerializeField] private GameObject _healthModulePrefab;
        private PreferredKeyboardBindingsAbilityUser _abilityUser;
        private ICommandModuleGraph  _graph;
        
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
                IModule module = Instantiate(_healthModulePrefab).GetComponent<IModuleContext>().GetModule();
                
                _graph.AddAndConnect(module,
                    module.Ports.FirstOrDefault(x => !x.IsConnected && x.Direction == PortDirection.Input),
                    _moduleEntity.GraphEditor.Graph.Modules.FirstOrDefault()?.Ports
                        .FirstOrDefault(x => !x.IsConnected && x.Direction == PortDirection.Output));
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