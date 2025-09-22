using System.Linq;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleEntityTestInput : MonoBehaviour
    {
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _distance;
        private IModuleEntity _moduleEntity;
        
        private void Awake()
        {
            _moduleEntity = GetComponent<IModuleEntity>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                IModule module = new HealthModifierModule(_maxHealth, _maxHealth);

                _moduleEntity.Graph.AddAndConnect(module,
                    module.Ports.FirstOrDefault(x => !x.IsConnected && x.Direction == PortDirection.Input), 
                    _moduleEntity.Graph.CoreModule.Ports.FirstOrDefault(x => !x.IsConnected && x.Direction == PortDirection.Output));
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                _moduleEntity.Graph.RemoveModule(_moduleEntity.Graph.Modules.LastOrDefault(x => x != _moduleEntity.Graph.CoreModule));
            }
        }
    }
}