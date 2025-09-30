using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IM.Graphs
{
    public class ModuleGraph : IModuleGraph
    {
        private readonly List<IConnection> _connections = new();
        private readonly List<IModule> _modules = new();

        public IReadOnlyList<INode> Nodes => _modules;
        public IReadOnlyList<IEdge> Edges => _connections;
        public IReadOnlyList<IModule> Modules => _modules;
        public IReadOnlyList<IConnection> Connections => _connections;
        
        public bool AddModule(IModule module)
        {
            if(module == null) throw new ArgumentNullException(nameof(module));
            
            if (Contains(module))
            {
                return false;
            }
            
            _modules.Add(module);
            return true;
        }

        public bool RemoveModule(IModule module)
        {
            if(module == null) throw new ArgumentNullException(nameof(module));
            
            if (!Contains(module))
            {
                return false;
            }
            
            foreach (IConnection connection in module.Ports.Select(p => p.Connection).Where(c => c != null))
            {
                Disconnect(connection);
            }

            return _modules.Remove(module);
        }
        
        public IConnection Connect(IModulePort output, IModulePort input)
        {
            if (output == null) throw new ArgumentNullException(nameof(output));
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (!_modules.Contains(output.Module) || !_modules.Contains(input.Module))
                throw new ArgumentException("Both modules must be added before connecting.");
            if(output.Module ==  input.Module)
                throw new ArgumentException("Cannot connect ports of the same module.");
            if(output.IsConnected || input.IsConnected)
                throw new ArgumentException("Port is already connected.");
            (output, input) = FixPorts(output, input);
            
            if (!output.CanConnect(input) || !input.CanConnect(output))
                return null;
            
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
            if (!connection.Input.CanDisconnect() || !connection.Output.CanDisconnect())
            {
                return;
            }
            
            connection.Input.Disconnect();
            connection.Output.Disconnect();
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
        
        private (IModulePort, IModulePort) FixPorts(IModulePort output, IModulePort input)
        {
            return output.Direction == PortDirection.Input ? (input, output) : (output, input);
        }
    }
}