using System.Collections.Generic;
using IM.Graphs;

namespace IM.ModuleGraph
{
    public class VisualModuleGraph : IVisualModuleGraph
    {
        private readonly List<IVisualModule> _visualModules = new();
        private readonly List<IConnection> _connections = new();

        public IReadOnlyList<INode> Nodes => _visualModules;
        public IReadOnlyList<IEdge> Edges => _connections;
        public IReadOnlyList<IModule> Modules => _visualModules;
        public IReadOnlyList<IConnection> Connections => _connections;
        
        public void AddModule(IVisualModule module)
        {
            throw new System.NotImplementedException();
        }

        public void AddAndConnect(IVisualModule module, IPort ownerPort, IPort targetPort)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveModule(IVisualModule module)
        {
            throw new System.NotImplementedException();
        }

        public IConnection Connect(IPort output, IPort input)
        {
            throw new System.NotImplementedException();
        }

        public void Disconnect(IConnection connection)
        {
            throw new System.NotImplementedException();
        }
    }
}