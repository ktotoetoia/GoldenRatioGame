using System;
using System.Collections.Generic;

namespace IM.Graphs
{
    public class ModuleGraphReadOnlyWrapper : IModuleGraphReadOnly
    {
        private readonly IModuleGraphReadOnly _graph;
        
        public IReadOnlyList<INode> Nodes => _graph.Nodes;
        public IReadOnlyList<IEdge> Edges => _graph.Edges;
        public IReadOnlyList<IConnection> Connections => _graph.Connections;
        public IReadOnlyList<IModule> Modules => _graph.Modules;

        public ModuleGraphReadOnlyWrapper(IModuleGraphReadOnly graph)
        {
            _graph = graph ?? throw new ArgumentNullException(nameof(graph));
        }
    }
}