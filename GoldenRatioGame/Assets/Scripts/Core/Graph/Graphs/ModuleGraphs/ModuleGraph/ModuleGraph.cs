using System;
using System.Collections.Generic;
using System.Linq;

namespace IM.Graphs
{
    public class ModuleGraph : IModuleGraph
    {
        private readonly List<IConnection> _connections = new();
        private readonly List<IModule> _modules = new();

        public IEnumerable<INode> Nodes => _modules;
        public IEnumerable<IEdge> Edges => _connections;
        public IEnumerable<IModule> Modules => _modules;
        public IEnumerable<IConnection> Connections => _connections;
        
        public void AddModule(IModule module)
        {
            if(module == null) throw new ArgumentNullException(nameof(module));
            
            if (Contains(module))
            {
                return;
            }
            
            _modules.Add(module);
        }

        public void AddAndConnect(IModule module, IPort ownerPort, IPort targetPort)
        {
            AddModule(module);
            Connect(ownerPort, targetPort);
        }

        public void RemoveModule(IModule module)
        {
            if(module == null) throw new ArgumentNullException(nameof(module));
            
            if (!Contains(module))
            {
                return;
            }
            
            foreach (IConnection connection in module.Ports.Select(p => p.Connection).Where(c => c != null))
            {
                Disconnect(connection);
            }
        }
        
        public IConnection Connect(IPort output, IPort input)
        {
            if (output == null) throw new ArgumentNullException(nameof(output));
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (!_modules.Contains(output.Module) || !_modules.Contains(input.Module))
                throw new ArgumentException("Both modules must be added before connecting.");
            if(output.Module ==  input.Module)
                throw new ArgumentException("Cannot connect ports of the same module.");
            if(output.IsConnected || input.IsConnected)
                throw new ArgumentException("Port is already connected.");
            
            Connection connection =  new Connection(output, input);

            input.Connect(connection);
            output.Connect(connection);
            _connections.Add(connection);

            return connection;
        }

        public void Disconnect(IConnection connection)
        {
            if(connection == null) throw new ArgumentNullException(nameof(connection));
            if (!Contains(connection)) throw new ArgumentException("Graph does not contain this connection.");
            
            connection.Port1.Disconnect();
            connection.Port2.Disconnect();
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
            foreach (var module in _modules.ToList())
            {
                RemoveModule(module);
            }
        }
    }
}