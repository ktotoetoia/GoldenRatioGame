using System;
using System.Collections.Generic;
using System.Linq;

namespace IM.Graphs
{
    public class ModuleGraph : IModuleGraph
    {
        private readonly List<IConnection> _connections = new();
        private readonly List<IModule> _modules = new();
        
        public IReadOnlyList<IModule> Modules => _modules;
        public IReadOnlyList<IConnection> Connections => _connections;

        public IReadOnlyList<INode> Nodes => _modules;
        public IReadOnlyList<IEdge> Edges => _connections;
        
        public bool AddModule(IModule module)
        {
            if (Contains(module))
            {
                return false;
            }
            
            _modules.Add(module);
            return true;
        }

        public bool RemoveModule(IModule module)
        {
            if (!_modules.Remove(module))
            {
                return false;
            }
            
            module.Ports
                .Select(p => p.Connection)
                .Where(c => c != null)
                .ToList()
                .ForEach(Disconnect);
            
            return true;
        }
        
        public IConnection Connect(IModulePort output, IModulePort input)
        {
            CheckPorts(output,input);
            
            Connection connection =  new Connection(output, input);

            connection.Connect();
            _connections.Add(connection);

            return connection;
        }

        public void Disconnect(IConnection connection)
        {
            if(connection == null) throw new ArgumentNullException(nameof(connection));
            if (!Contains(connection)) throw new ArgumentException("Graph does not contain this connector");
            
            connection.Disconnect();
            _connections.Remove(connection);
        }

        public bool Contains(IModule module)
        {
            return _modules.Contains(module);
        }

        public bool Contains(IConnection connection)
        {
            return _connections.Contains(connection);
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