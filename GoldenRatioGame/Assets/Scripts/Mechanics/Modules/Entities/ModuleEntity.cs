using IM.Abilities;
using IM.Graphs;
using IM.Values;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleEntity : MonoBehaviour, IModuleEntity
    {
        [SerializeField] private CappedValue<float> _floatHealth;

        public GameObject GameObject => gameObject;
        public IModuleGraphEditor GraphEditor { get; private set; }
        public IAbilityPool AbilityPool { get; private set; }

        private void Awake()
        {
            HumanoidCoreModule coreModule = new HumanoidCoreModule(_floatHealth);
            
            ICommandModuleGraph graph = new CommandModuleGraph();
            graph.AddModule(coreModule);
            GraphEditor = new ModuleGraphEditor(graph);

            AbilityPool = new AbilityPool();

            //_graph.AddObserver(new EntityInjector(this));
            //_graph.AddObserver(new SpeedExtensionsObserver(GetComponent<IHaveSpeed>()));
            //_graph.AddObserver(new HealthExtensionsObserver(GetComponent<IFloatHealthValuesGroup>()));
            //_graph.AddObserver(new AbilityExtensionsObserver(AbilityPool as AbilityPool));
        }
    }
}