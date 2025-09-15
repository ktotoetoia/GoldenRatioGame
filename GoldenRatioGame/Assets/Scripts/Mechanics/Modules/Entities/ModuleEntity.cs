using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleEntity : MonoBehaviour, IModuleEntity
    {
        [SerializeField] private float _maxHealth;
        private readonly List<IModule> _modulesUsed = new();
        private CoreModuleGraphEvents _graph;
        
        public GameObject GameObject => gameObject;
        public ICoreModuleGraph Graph => _graph;
        
        private void Awake()
        {
            HumanoidCoreModule coreModule = new HumanoidCoreModule(_maxHealth, _maxHealth);
            _graph = new CoreModuleGraphEvents(coreModule);

            coreModule.Entity = this;

            _graph.OnGraphChange += U;
            _graph.SetCoreModule(coreModule);
            U();
        }

        private void U()
        {
            List<IModule> newModules = Graph
                .GetCoreSubgraph()
                .Nodes
                .OfType<IModule>()
                .ToList();
            
            HashSet<IModule> oldSet = new HashSet<IModule>(_modulesUsed);
            
            foreach (IModule added in newModules)
            {
                if (oldSet.Remove(added)) 
                    continue;

                if (added is IEntityHolder h)
                {
                    h.Entity = this;
                }
            }

            foreach (IModule removed in oldSet)
            {
                if (removed is IEntityHolder h)
                {
                    h.Entity = null;
                }
            }

            _modulesUsed.Clear();
            _modulesUsed.AddRange(newModules);
        }
    }
}