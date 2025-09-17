using System.Collections.Generic;
using IM.Abilities;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleEntity : MonoBehaviour, IModuleEntity
    {
        [SerializeField] private float _maxHealth;
        private List<IGraphObserver>  _graphObservers = new();
        private CoreModuleGraphEvents _graph;
        private AbilitiesObserver _abilitiesObserver;
        
        public GameObject GameObject => gameObject;
        public ICoreModuleGraph Graph => _graph;
        public IAbilitiesPool AbilitiesPool => _abilitiesObserver;
        
        private void Awake()
        {
            HumanoidCoreModule coreModule = new HumanoidCoreModule(_maxHealth, _maxHealth);
            _graph = new CoreModuleGraphEvents(coreModule);
            _abilitiesObserver = new AbilitiesObserver(_graph);
            
            _graphObservers = new List<IGraphObserver>()
            {
                new HealthModulesObserver(this),
                _abilitiesObserver,
            };

            _graph.OnGraphChange += UpdateObservers;
            _graph.SetCoreModule(coreModule);
            GetComponent<AbilitiesUserMono>().Pool = AbilitiesPool;
        }

        private void UpdateObservers()
        {
            _graphObservers.ForEach(x => x.OnGraphChange());
        }
    }
}