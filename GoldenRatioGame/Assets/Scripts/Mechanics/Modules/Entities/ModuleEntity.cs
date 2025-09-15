using System.Collections.Generic;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleEntity : MonoBehaviour, IModuleEntity
    {
        [SerializeField] private float _maxHealth;
        private List<IGraphObserver>  _graphObservers = new();
        private CoreModuleGraphEvents _graph;
        
        public GameObject GameObject => gameObject;
        public ICoreModuleGraph Graph => _graph;
        
        private void Awake()
        {
            HumanoidCoreModule coreModule = new HumanoidCoreModule(_maxHealth, _maxHealth);
            _graph = new CoreModuleGraphEvents(coreModule);
            _graphObservers = new List<IGraphObserver>()
            {
                new HealthModulesObserver(this),
            };

            _graph.OnGraphChange += UpdateObservers;
            _graph.SetCoreModule(coreModule);
            
            UpdateObservers();
        }

        private void UpdateObservers()
        {
            _graphObservers.ForEach(x => x.OnGraphChange());
        }
    }
}