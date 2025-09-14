using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleEntity : MonoBehaviour, IModuleEntity
    {
        [SerializeField] private float _maxHealth;
        private CoreModuleGraphEvents _graph;
        private List<IModulesObserver> _graphReaders;
        
        public GameObject GameObject => gameObject;
        public ICoreModuleGraph Graph => _graph;

        private void Awake()
        {
            HumanoidCoreModule coreModule = new HumanoidCoreModule(this, _maxHealth, _maxHealth);
            _graph = new CoreModuleGraphEvents(coreModule);

            _graph.OnGraphChange += coreModule.OnStructureUpdated;
            
            coreModule.OnStructureUpdated();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                IModule module = new HealthModifyingModule(_maxHealth, _maxHealth);
                Graph.AddModule(module);
                Graph.Connect(module.Ports.FirstOrDefault(x => !x.IsConnected && x.Direction == PortDirection.Input), Graph.CoreModule.Ports.FirstOrDefault(x => !x.IsConnected && x.Direction == PortDirection.Output));
            }

            if (Input.GetMouseButtonDown(1))
            {
                Graph.RemoveModule(Graph.Modules.FirstOrDefault(x => x != Graph.CoreModule));
            }
        }
    }
}