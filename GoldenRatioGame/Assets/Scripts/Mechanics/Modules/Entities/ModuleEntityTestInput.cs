using System.Linq;
using IM.Abilities;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleEntityTestInput : MonoBehaviour
    {
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _distance;
        private IModuleEntity _moduleEntity;
        private IAbility _ability;
        
        private void Awake()
        {
            _moduleEntity = GetComponent<IModuleEntity>();
            
            _ability = new BlinkForwardAbility(
                GetDirection,
                () => transform,
                0);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                _ability.TryUse();
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                IModule module = new HealthModifierModule(_maxHealth, _maxHealth);
                _moduleEntity.Graph.AddModule(module);
                _moduleEntity.Graph.Connect(module.Ports.FirstOrDefault(x => !x.IsConnected && x.Direction == PortDirection.Input), _moduleEntity.Graph.CoreModule.Ports.FirstOrDefault(x => !x.IsConnected && x.Direction == PortDirection.Output));
            }

            if (Input.GetMouseButtonDown(1))
            {
                _moduleEntity.Graph.RemoveModule(_moduleEntity.Graph.Modules.FirstOrDefault(x => x != _moduleEntity.Graph.CoreModule));
            }
        }

        private Vector2 GetDirection()
        {
            return ((Vector2)(Camera.main.ScreenToWorldPoint(Input.mousePosition)-transform.position)).normalized * _distance;
        }
    }
}