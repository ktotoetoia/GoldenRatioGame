using System.Collections.Generic;
using System.Linq;
using IM.Entities;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class EntityModuleGraphReader : IGraphReader
    {
        private readonly List<IEntityModule> _modules = new();
        public IModuleGraph Graph { get; }
        public IEntity Entity { get; }

        public EntityModuleGraphReader(IModuleGraph graph, IEntity entity)
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