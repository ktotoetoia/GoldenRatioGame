using System.Collections.Generic;
using System.Linq;
using IM.Entities;
using IM.Graphs;
using IM.Health;

namespace IM.Modules
{
    public class HealthModulesGraphObserver: IGraphObserver
    {
        private readonly List<IHealthModule> _modules = new();
        private IFloatHealthValuesGroup _floatHealthValuesGroup;
        
        public ICoreModuleGraph Graph { get; }
        public IEntity Entity { get; }

        public IFloatHealthValuesGroup HealthValuesGroup
        {
            get
            {
                return _floatHealthValuesGroup ??= Entity.GameObject.GetComponent<IFloatHealthValuesGroup>();
            }
        }
        public HealthModulesGraphObserver(ICoreModuleGraph graph, IEntity entity)
        {
            Graph = graph;
            Entity = entity;
        }
        
        public void OnGraphStructureChanged()
        {
            List<IHealthModule> newModules = Graph
                .GetCoreSubgraph()
                .Nodes
                .OfType<IHealthModule>()
                .ToList();

            HashSet<IHealthModule> oldSet = new HashSet<IHealthModule>(_modules);

            foreach (IHealthModule added in newModules)
            {
                if (oldSet.Remove(added)) 
                    continue;

                HealthValuesGroup.AddHealth(added.GetHealth());
            }

            foreach (IHealthModule removed in oldSet)
            {
                HealthValuesGroup.RemoveHealth(removed.GetHealth());
            }

            _modules.Clear();
            _modules.AddRange(newModules);
        }
    }
}