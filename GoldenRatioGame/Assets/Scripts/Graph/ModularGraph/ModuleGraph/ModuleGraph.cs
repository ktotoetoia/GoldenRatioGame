using System;
using System.Collections.Generic;
using System.Linq;

namespace IM.Graphs
{
    public class ModuleGraph : IModuleGraph
    {
        private readonly List<ModuleConnection> _connections = new();
        private readonly List<IModule> _modules = new();
        
        public IEnumerable<IModule> Modules => _modules;
        public IEnumerable<IModuleConnection> Connections => _connections;

        public IReadOnlyList<INode> Nodes => _modules;
        public IReadOnlyList<IEdge> Edges => _connections;
        
        public void AddModule(IModule module)
        {
            if (_modules.Contains(module))
            {
                throw new ArgumentException($"Graph already contains this module: {module}");
            }
            
            _modules.Add(module);
        }

        public void RemoveModule(IModule module)
        {
            if (!_modules.Remove(module))
                throw new InvalidOperationException("Module not in graph.");

            List<ModuleConnection> toRemove = module.Ports
                .Select(p => p.Connection)
                .Where(c => c != null)
                .Cast<ModuleConnection>()
                .ToList();

            foreach (ModuleConnection c in toRemove)
            {
                Disconnect(c);
            }
        }
        
        public IModuleConnection Connect(IModulePort from, IModulePort to)
        {
            if (from == null) throw new ArgumentNullException(nameof(from));
            if (to == null) throw new ArgumentNullException(nameof(to));
            
            if (!_modules.Contains(from.Module) || !_modules.Contains(to.Module))
                throw new ArgumentException("modules must be add before connecting");
            if (from.Direction != PortDirection.Output || to.Direction != PortDirection.Input)
                throw new ArgumentException("from port must be output and to port must be input");
            
            ModuleConnection connection =  new ModuleConnection(from, to);
            
            if (!from.CanConnect(connection) || !to.CanConnect(connection))
                throw new InvalidOperationException("Ports refused the connection.");

            connection.Connect();
            _connections.Add(connection);

            return connection;
        }

        public void Disconnect(IModuleConnection connection)
        {
            if (connection is not ModuleConnection moduleConnector || !_connections.Contains(moduleConnector))
                throw new ArgumentException("Graph does not contain this connector");
            
            if (connection.Input.CanDisconnect() && connection.Output.CanDisconnect())
            {
                moduleConnector.Disconnect();
                _connections.Remove(moduleConnector);
            }
        }
    }
}