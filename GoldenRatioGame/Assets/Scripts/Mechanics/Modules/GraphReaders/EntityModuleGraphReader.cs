using System.Collections.Generic;
using System.Linq;
using IM.Entities;
using IM.Graphs;

namespace IM.Modules
{
    public class EntityModuleGraphReader : IGraphReader
    {
        private readonly List<IEntityModule> _modules = new();
        public ICoreModuleGraph Graph { get; }
        public IEntity Entity { get; }

        public EntityModuleGraphReader(ICoreModuleGraph graph, IEntity entity)
        {
            Graph = graph;
            Entity = entity;
        }
        
        public void OnGraphStructureChanged()
        {
            foreach (IEntityModule module in _modules)
            {
                module.RemoveFromBuild(Entity);
            }
            
            _modules.Clear();
            _modules.AddRange(Graph.Modules.OfType<IEntityModule>());

            foreach (IEntityModule module in _modules)
            {
                module.AddToBuild(Entity);
            }
        }
    }
}