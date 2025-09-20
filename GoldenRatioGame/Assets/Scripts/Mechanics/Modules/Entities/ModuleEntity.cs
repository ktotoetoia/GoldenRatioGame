using System.Collections.Generic;
using IM.Abilities;
using IM.Graphs;
using IM.Health;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleEntity : MonoBehaviour, IModuleEntity
    {
        [SerializeField] private float _maxHealth;
        private List<IModuleObserver>  _moduleObservers = new();
        private CoreModuleGraphEvents _graph;
        private AbilitiesObserver _abilitiesObserver;
        
        public GameObject GameObject => gameObject;
        public ICoreModuleGraph Graph => _graph;
        public IAbilitiesPool AbilitiesPool => _abilitiesObserver;
        
        private void Awake()
        {
            HumanoidCoreModule coreModule = new HumanoidCoreModule(_maxHealth, _maxHealth);
            _graph = new CoreModuleGraphEvents(coreModule);
            _abilitiesObserver = new AbilitiesObserver();
            
            _moduleObservers = new List<IModuleObserver>()
            {
                new HealthModulesObserver(GetComponent<IFloatHealthValuesGroup>()),
                _abilitiesObserver,
            };

            _graph.OnModuleAdded += x =>
            {
                foreach (IModuleObserver moduleObserver in _moduleObservers)
                {
                    moduleObserver.Add(x);
                }
            };

            _graph.OnModuleRemoved += x =>
            {
                foreach (IModuleObserver moduleObserver in _moduleObservers)
                {
                    moduleObserver.Remove(x);
                }
            };
            
            _graph.SetCoreModule(coreModule);
            GetComponent<AbilitiesUserMono>().Pool = AbilitiesPool;
        }
    }
}