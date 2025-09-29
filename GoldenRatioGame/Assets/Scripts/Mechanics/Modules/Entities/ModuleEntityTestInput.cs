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
        
        private void Awake()
        {
            _abilityUser = new PreferredKeyboardBindingsAbilityUser(_moduleEntity.AbilityPool);
        }

        private void Update()
        {
            _abilityUser.Update();
            if (Input.GetKeyDown(KeyCode.O))
            {
                IModule module = Instantiate(_healthModulePrefab).GetComponent<IModule>();

                _moduleEntity.Graph.AddAndConnect(module,
                    module.Ports.FirstOrDefault(x => !x.IsConnected && x.Direction == PortDirection.Input), 
                    _moduleEntity.Graph.Modules.FirstOrDefault()?.Ports.FirstOrDefault(x => !x.IsConnected && x.Direction == PortDirection.Output));
            }

            if (Input.GetKeyDown(KeyCode.U))
            {
                IModule module = new BlinkAbilityModule();

                _moduleEntity.Graph.AddAndConnect(module,
                    module.Ports.FirstOrDefault(x => !x.IsConnected && x.Direction == PortDirection.Input),
                    _moduleEntity.Graph.Modules.FirstOrDefault()?.Ports.FirstOrDefault(x =>
                        !x.IsConnected && x.Direction == PortDirection.Output));
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                _moduleEntity.Graph.RemoveModule(_moduleEntity.Graph.Modules.LastOrDefault(x => x != _moduleEntity.Graph.Modules.FirstOrDefault()));
            }
        }
    }
}