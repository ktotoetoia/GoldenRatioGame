using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleEntity : MonoBehaviour, IModuleEntity
    {
        [SerializeField] private float _maxHealth;
        private CoreModuleGraph _graph;
        private List<IGraphObserver> _graphReaders;
        
        public GameObject GameObject => gameObject;
        public ICoreModuleGraph Graph => _graph;

        private void Awake()
        {
            ModuleGraphEvents graphEvents = new ModuleGraphEvents(new SafeModuleGraph());
            
            _graph = new CoreModuleGraph(new HealthModifyingModule(_maxHealth,_maxHealth),graphEvents);

            graphEvents.OnGraphChanged +=UpdateGraphReaders;
        
            _graphReaders = new List<IGraphObserver>(0)
            {
                new HealthModulesGraphObserver(_graph,this),
            };

            UpdateGraphReaders();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                IModule module = new HealthModifyingModule(_maxHealth, _maxHealth);
                _graph.AddModule(module);
                _graph.Connect(module.Ports.FirstOrDefault(x => !x.IsConnected && x.Direction == PortDirection.Input), _graph.CoreModule.Ports.FirstOrDefault(x => !x.IsConnected && x.Direction == PortDirection.Output));
            }

            if (Input.GetMouseButtonDown(1))
            {
                _graph.RemoveModule(_graph.Modules.FirstOrDefault(x => x != _graph.CoreModule));
            }
        }

        private void UpdateGraphReaders()
        {
            _graphReaders.ForEach(x => x.OnGraphStructureChanged());
        }
    }
}