using System;
using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.Health;

namespace IM.Modules
{
    public class HealthModulesObserver : IGraphObserver
    {
        private readonly List<IHealthModule> _modulesUsed = new();
        private readonly IFloatHealthValuesGroup _floatHealthValuesGroup;
        private ICoreModuleGraph _graph;
        
        public ICoreModuleGraph Graph
        {
            get => _graph;
            private set
            {
                _graph = value;
                OnGraphChange();
            }
        }
        
        public HealthModulesObserver(IModuleEntity entity)
        {
            if (!entity.GameObject.TryGetComponent(out _floatHealthValuesGroup))
            {
                throw new Exception();
            }
            
            Graph = entity.Graph;
        }

        public void OnGraphChange()
        {
            List<IHealthModule> newModules =_graph
                .GetCoreSubgraph()
                .Nodes
                .OfType<IHealthModule>()
                .ToList();
            
            HashSet<IHealthModule> oldSet = new HashSet<IHealthModule>(_modulesUsed);
            
            foreach (IHealthModule added in newModules)
            {
                if (oldSet.Remove(added)) 
                    continue;
                    
                _floatHealthValuesGroup.AddHealth(added.Health);
            }

            foreach (IHealthModule removed in oldSet)
            {
                _floatHealthValuesGroup.RemoveHealth(removed.Health);
            }

            _modulesUsed.Clear();
            _modulesUsed.AddRange(newModules);
        }
    }
}