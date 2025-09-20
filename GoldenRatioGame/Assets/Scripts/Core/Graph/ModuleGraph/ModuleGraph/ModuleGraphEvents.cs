using System;
using System.Collections.Generic;

namespace IM.Graphs
{
    public class ModuleGraphEvents : IModuleGraph, IModuleGraphEvents
    {
        private readonly IModuleGraph _graph;
        
        public IReadOnlyList<INode> Nodes => _graph.Nodes;
        public IReadOnlyList<IEdge> Edges => _graph.Edges;
        public IReadOnlyList<IModule> Modules => _graph.Modules;
        public IReadOnlyList<IModuleConnection> Connections => _graph.Connections;
        
        public event Action<IModule> OnModuleAdded;
        public event Action<IModule> OnModuleRemoved;
        public event Action<IModuleConnection> OnConnected;
        public event Action<IModuleConnection> OnDisconnected;
        public event Action OnGraphChanged;
        
        public ModuleGraphEvents() : this(new ModuleGraph())
        {
            
        }
        
        public ModuleGraphEvents(IModuleGraph graph)
        {
            _graph = graph;
        }

        public void AddModule(IModule module)
        {
            _graph.AddModule(module);
            CallOnModuleAdded(module);
        }

        public void RemoveModule(IModule module)
        {
            _graph.RemoveModule(module);
            CallOnModuleRemoved(module);
        }

        public IModuleConnection Connect(IModulePort output, IModulePort input)
        {
            IModuleConnection connection = _graph.Connect(output, input);
            
            CallOnConnected(connection);

            return connection;
        }

        public void Disconnect(IModuleConnection connection)
        {
            _graph.Disconnect(connection);
            CallOnDisconnected(connection);
        }

        protected void CallOnGraphChanged()
        {
            OnGraphChanged?.Invoke();
        }

        protected void CallOnModuleAdded(IModule module)
        {
            OnModuleAdded?.Invoke(module);
            CallOnGraphChanged();
        }

        protected void CallOnModuleRemoved(IModule module)
        {
            OnModuleRemoved?.Invoke(module);
            CallOnGraphChanged();
        }

        protected void CallOnConnected(IModuleConnection connection)
        {
            OnConnected?.Invoke(connection);
            CallOnGraphChanged();
        }

        protected void CallOnDisconnected(IModuleConnection connection)
        {
            OnDisconnected?.Invoke(connection);
            CallOnGraphChanged();
        }
    }
}