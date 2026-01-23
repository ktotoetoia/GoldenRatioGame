using System;
using System.Collections.Generic;

namespace IM.Graphs
{
    public class ModuleGraphReadOnlyWrapper : IModuleGraphReadOnly
    {
        private readonly IModuleGraphReadOnly _graph;
        
        public IEnumerable<INode> Nodes => _graph.Nodes;
        public IEnumerable<IEdge> Edges => _graph.Edges;
        public IEnumerable<IConnection> Connections => _graph.Connections;
        public IEnumerable<IModule> Modules => _graph.Modules;

        public ModuleGraphReadOnlyWrapper(IModuleGraphReadOnly graph)
        {
            _graph = graph ?? throw new ArgumentNullException(nameof(graph));
        }
        
        public bool Contains(IModule module)
        {
            return _graph.Contains(module);
        }

        public bool Contains(IConnection connection)
        {
            return _graph.Contains(connection);
        }
    }
}