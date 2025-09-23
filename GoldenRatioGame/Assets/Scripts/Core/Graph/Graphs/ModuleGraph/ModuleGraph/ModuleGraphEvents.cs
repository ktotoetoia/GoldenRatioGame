using System;
using System.Collections.Generic;

namespace IM.Graphs
{
    public sealed class ModuleGraphEvents : IModuleGraph, IModuleGraphEvents
    {
        private readonly IModuleGraph _graph;
        
        public IReadOnlyList<INode> Nodes => _graph.Nodes;
        public IReadOnlyList<IEdge> Edges => _graph.Edges;
        public IReadOnlyList<IModule> Modules => _graph.Modules;
        public IReadOnlyList<IConnection> Connections => _graph.Connections;
        
        public event Action<IModule> OnModuleAdded;
        public event Action<IModule> OnModuleRemoved;
        public event Action<IConnection> OnConnected;
        public event Action<IConnection> OnDisconnected;
        public event Action OnGraphChanged;
        
        public ModuleGraphEvents() : this(new ModuleGraph())
        {
            
        }
        
        public ModuleGraphEvents(IModuleGraph graph)
        {
            _graph = graph;
        }

        public bool AddModule(IModule module)
        {
            if (_graph.AddModule(module))
            {
                OnModuleAdded?.Invoke(module);
                OnGraphChanged?.Invoke();

                return true;
            }

            return false;
        }

        public bool RemoveModule(IModule module)
        {
            if (_graph.RemoveModule(module))
            {
                OnModuleRemoved?.Invoke(module);
                OnGraphChanged?.Invoke();

                return true;
            }
            
            return false;
        }

        public IConnection Connect(IModulePort output, IModulePort input)
        {
            IConnection connection = _graph.Connect(output, input);
            
            OnConnected?.Invoke(connection);
            OnGraphChanged?.Invoke();

            return connection;
        }

        public void Disconnect(IConnection connection)
        {
            _graph.Disconnect(connection);
            OnDisconnected?.Invoke(connection);
            OnGraphChanged?.Invoke();
        }
    }
}