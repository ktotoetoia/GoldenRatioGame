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
            if (Contains(module))
                throw new ArgumentException($"Graph already contains this module: {module}");
            
            _modules.Add(module);
        }

        public void RemoveModule(IModule module)
        {
            if (!_modules.Remove(module))
                throw new InvalidOperationException("Module not in graph.");

            module.Ports
                .Select(p => p.Connection)
                .Where(c => c != null)
                .OfType<ModuleConnection>()
                .ToList()
                .ForEach(Disconnect);
        }
        
        public IModuleConnection Connect(IModulePort output, IModulePort input)
        {
            CheckPorts(output,input);
            
            ModuleConnection connection =  new ModuleConnection(output, input);

            if (!connection.CanConnect())
                throw new InvalidOperationException("Ports refused the connection.");

            connection.Connect();
            _connections.Add(connection);

            return connection;
        }

        public void Disconnect(IModuleConnection connection)
        {
            if(connection == null) throw new ArgumentNullException(nameof(connection));
            if (!Contains(connection)) throw new ArgumentException("Graph does not contain this connector");

            if (connection is not ModuleConnection moduleConnection || moduleConnection.CanDisconnect())
                throw new InvalidOperationException("Cannot disconnect module connection.");
            
            
            moduleConnection.Disconnect();
            _connections.Remove(moduleConnection);
        }

        public bool Contains(IModule module)
        {
            return _modules.Contains(module);
        }

        public bool Contains(IModuleConnection connection)
        {
            return connection is ModuleConnection moduleConnection && _connections.Contains(moduleConnection);
        }

        public void Clear()
        {
            List<IModule> modulesCopy = _modules.ToList();

            foreach (IModule module in modulesCopy)
            {
                RemoveModule(module);
            }
            
            _modules.Clear();
            _connections.Clear();
        }

        private void CheckPorts(IModulePort output, IModulePort input)
        {
            if (output == null) throw new ArgumentNullException(nameof(output));
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (!_modules.Contains(output.Module) || !_modules.Contains(input.Module))
                throw new ArgumentException("modules must be add before connecting");
            if (output.Direction != PortDirection.Output || input.Direction != PortDirection.Input)
                throw new ArgumentException("from port must be output and to port must be input");
            if(output.Module ==  input.Module)
                throw new ArgumentException("Cannot connect ports of the same module.");
            if (_connections.Any(c =>
                    c.Output == output && c.Input == input))
                throw new InvalidOperationException("These ports are already connected.");
        }
    }
}