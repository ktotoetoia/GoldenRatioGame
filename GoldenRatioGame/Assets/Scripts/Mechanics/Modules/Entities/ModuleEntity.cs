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
        private ObservableModuleGraph _graph;

        public GameObject GameObject => gameObject;
        public IObservableModuleGraph Graph => _graph;
        public IAbilityPool AbilityPool { get; private set; }
        
        private void Awake()
        {
            HumanoidCoreModule coreModule = new HumanoidCoreModule(_floatHealth);
            _graph = new ObservableModuleGraph(coreModule);
            AbilityPool = new AbilityPool();
            
            _graph.AddObserver(new LookPositionProviderInjector());
            _graph.AddObserver(new EntityInjector(this));
            _graph.AddObserver(new HealthExtensionsObserver(GetComponent<IFloatHealthValuesGroup>()));
            _graph.AddObserver(new AbilityExtensionsObserver(AbilityPool as AbilityPool));
        }
    }
}