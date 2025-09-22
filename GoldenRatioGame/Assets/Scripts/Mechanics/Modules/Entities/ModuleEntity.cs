using IM.Abilities;
using IM.Graphs;
using IM.Health;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleEntity : MonoBehaviour, IModuleEntity
    {
        [SerializeField] private float _maxHealth;

        private GameModuleGraph _graph;
        private AbilitiesObserver _abilitiesObserver;
        public GameObject GameObject => gameObject;
        public IGameModuleGraph Graph => _graph;
        public IAbilitiesPool AbilitiesPool => _abilitiesObserver;
        
        private void Awake()
        {
            HumanoidCoreModule coreModule = new HumanoidCoreModule(_maxHealth, _maxHealth);
            
            _graph = new GameModuleGraph(coreModule);
            
            _abilitiesObserver = new AbilitiesObserver();
            _graph.AddObserver(new HealthModulesObserver(GetComponent<IFloatHealthValuesGroup>()));
            _graph.AddObserver(_abilitiesObserver);
            
            GetComponent<AbilitiesUserMono>().Pool = AbilitiesPool;
        }
    }
}