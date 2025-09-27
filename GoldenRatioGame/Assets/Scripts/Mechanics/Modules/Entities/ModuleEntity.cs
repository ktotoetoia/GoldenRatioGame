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
        private AbilitiesObserver _abilitiesObserver;

        public GameObject GameObject => gameObject;
        public IObservableModuleGraph Graph => _graph;
        public IAbilitiesPool AbilitiesPool => _abilitiesObserver;
        
        private void Awake()
        {
            HumanoidCoreExtension coreExtension = new HumanoidCoreExtension(_floatHealth);
            _graph = new ObservableModuleGraph(coreExtension);
            
            _abilitiesObserver = new AbilitiesObserver();
            _graph.AddObserver(new HealthExtensionsObserver(GetComponent<IFloatHealthValuesGroup>()));
            _graph.AddObserver(_abilitiesObserver);
            
            GetComponent<AbilitiesUserMono>().Pool = AbilitiesPool;
        }
    }
}