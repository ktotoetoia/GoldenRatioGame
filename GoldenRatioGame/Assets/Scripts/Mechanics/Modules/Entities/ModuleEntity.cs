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
        [SerializeField] private GameObject _coreModulePrefab;
        [SerializeField] private CappedValue<float> _floatHealth;

        public GameObject GameObject => gameObject;
        public IModuleGraphEditor<IConditionalCommandModuleGraph> GraphEditor { get; private set; }
        public IAbilityPool AbilityPool { get; private set; }

        private void Awake()
        {
            IModule coreModule = Instantiate(_coreModulePrefab).GetComponent<IModule>();
            AbilityPool = new AbilityPool();
            ConditionalCommandModuleGraph graph = new ConditionalCommandModuleGraph();

            List<IModuleGraphObserver> graphObservers = new List<IModuleGraphObserver>()
            {
                new EntityInjector(this),
                new SpeedExtensionsObserver(GetComponent<IHaveSpeed>()),
                new HealthExtensionsObserver(GetComponent<IFloatHealthValuesGroup>()),
                new AbilityExtensionsObserver(AbilityPool as AbilityPool),
            };
            
            graphObservers.AddRange(GetComponents<IModuleGraphObserver>());
            
            CompositeObserver observer = new CompositeObserver(graphObservers);
            
            GraphEditor = new CommandModuleGraphEditor<IConditionalCommandModuleGraph>(graph,new AccessConditionalCommandModuleGraphFactory(), new TrueModuleGraphValidator(), observer);
            graph.AddModule(coreModule);
        }
    }
}