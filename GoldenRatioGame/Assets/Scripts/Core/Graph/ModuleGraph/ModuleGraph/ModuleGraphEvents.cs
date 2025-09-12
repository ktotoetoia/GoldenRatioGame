using System;
using System.Collections.Generic;

namespace IM.Graphs
{
    public class ModuleGraphEvents : IModuleGraph, IModuleGraphEvents
    {
        protected readonly IModuleGraph _graph;
        
        public IReadOnlyList<INode> Nodes => _graph.Nodes;
        public IReadOnlyList<IEdge> Edges => _graph.Edges;
        public IEnumerable<IModule> Modules => _graph.Modules;
        public IEnumerable<IModuleConnection> Connections => _graph.Connections;
        
        public event Action<IModule> OnModuleAdded;
        public event Action<IModule> OnModuleRemoved;
        public event Action<IModuleConnection> OnConnected;
        public event Action<IModuleConnection> OnDisconnected;
        public event Action OnGraphChange;
        
        public ModuleGraphEvents() : this(new ModuleGraph())
        {
            
        }
        
        public ModuleGraphEvents(IModuleGraph graph)
        {
            _graph = graph;
            
            OnModuleAdded += x => OnGraphChanged();
            OnModuleRemoved += x => OnGraphChanged();
            OnConnected += x => OnGraphChanged();
            OnDisconnected += x => OnGraphChanged();
        }

        public void AddModule(IModule module)
        {
            _graph.AddModule(module);
            OnModuleAdded?.Invoke(module);
        }

        public void RemoveModule(IModule module)
        {
            _graph.RemoveModule(module);
            OnModuleRemoved?.Invoke(module);
        }

        public IModuleConnection Connect(IModulePort output, IModulePort input)
        {
            IModuleConnection connection = _graph.Connect(output, input);
            
            OnConnected?.Invoke(connection);

            return connection;
        }

        public void Disconnect(IModuleConnection connection)
        {
            _graph.Disconnect(connection);
            OnDisconnected?.Invoke(connection);
        }

        protected void OnGraphChanged()
        {
            OnGraphChange?.Invoke();
        }
    }
}