using System.Collections.Generic;
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

        public GameObject GameObject => gameObject;
        public IModuleGraphEditor<ICommandModuleGraph> GraphEditor { get; private set; }
        public IAbilityPool AbilityPool { get; private set; }

        private void Awake()
        {
            HumanoidCoreModule coreModule = new HumanoidCoreModule(_floatHealth);
            AbilityPool = new AbilityPool();
            CommandModuleGraph graph = new CommandModuleGraph();
            
            CompositeObserver observer = new CompositeObserver(new List<IModuleGraphObserver>()
            {
                new EntityInjector(this),
                new SpeedExtensionsObserver(GetComponent<IHaveSpeed>()),
                new HealthExtensionsObserver(GetComponent<IFloatHealthValuesGroup>()),
                new AbilityExtensionsObserver(AbilityPool as AbilityPool),
            });
            
            GraphEditor = new CommandModuleGraphEditor(graph, new TrueModuleGraphValidator(), observer);
            graph.AddModule(coreModule);
        }
    }
}