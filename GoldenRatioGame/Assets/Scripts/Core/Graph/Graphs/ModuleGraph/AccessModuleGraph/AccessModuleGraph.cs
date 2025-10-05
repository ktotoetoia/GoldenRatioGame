using System;
using System.Collections.Generic;

namespace IM.Graphs
{
    public class AccessModuleGraph : IAccessModuleGraph
    {
        private readonly IModuleGraph _graph;
        public IReadOnlyList<INode> Nodes => _graph.Nodes;
        public IReadOnlyList<IEdge> Edges => _graph.Edges;
        public IReadOnlyList<IConnection> Connections => _graph.Connections;
        public IReadOnlyList<IModule> Modules => _graph.Modules;

        public bool CanUse { get; set; }
        public bool ThrowIfCantUse { get; set; }
        
        public AccessModuleGraph() : this(new ModuleGraph())
        {
            
        }

        public AccessModuleGraph(IModuleGraph graph)
        {
            _graph = graph;
        }
        
        public void AddModule(IModule module)
        {
            if(!TryUse()) return;
            
            _graph.AddModule(module);
        }

        public void RemoveModule(IModule module)
        {
            if(!TryUse()) return;
            
            _graph.RemoveModule(module);
        }

        public IConnection Connect(IModulePort output, IModulePort input)
        {
            if(!TryUse()) return null;
            
            return _graph.Connect(output, input);
        }

        public void Disconnect(IConnection connection)
        {
            if(!TryUse()) return;
            
            _graph.Disconnect(connection);
        }

        private bool TryUse()
        {
            if (!CanUse && ThrowIfCantUse) throw new InvalidOperationException("Cant use this graph");
            
            return CanUse;
        }
    }
}