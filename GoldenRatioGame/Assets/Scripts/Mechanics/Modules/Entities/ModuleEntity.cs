using IM.Abilities;
using IM.Graphs;
using IM.Health;
using IM.Values;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleEntity : MonoBehaviour, IModuleEntity
    {
        [SerializeField] private CappedValue<float> _floatHealth;
        private GameModuleGraph _graph;

        public GameObject GameObject => gameObject;
        public IGameModuleGraph Graph => _graph;
        public IAbilityPool AbilityPool { get; private set; }
        
        private void Awake()
        {
            HumanoidCoreModule coreModule = new HumanoidCoreModule(_floatHealth);
            _graph = new GameModuleGraph();
            _graph.AddModule(coreModule);
            
            AbilityPool = new AbilityPool();
            
            _graph.AddObserver(new EntityInjector(this));
            _graph.AddObserver(new HealthExtensionsObserver(GetComponent<IFloatHealthValuesGroup>()));
            _graph.AddObserver(new AbilityExtensionsObserver(AbilityPool as AbilityPool));
        }
    }
}