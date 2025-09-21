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
                CallOnModuleAdded(module);

                return true;
            }

            return false;
        }

        public bool RemoveModule(IModule module)
        {
            if (_graph.RemoveModule(module))
            {
                CallOnModuleRemoved(module);

                return true;
            }
            
            return false;
        }

        public IConnection Connect(IModulePort output, IModulePort input)
        {
            IConnection connection = _graph.Connect(output, input);
            
            CallOnConnected(connection);

            return connection;
        }

        public void Disconnect(IConnection connection)
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

        protected void CallOnConnected(IConnection connection)
        {
            OnConnected?.Invoke(connection);
            CallOnGraphChanged();
        }

        protected void CallOnDisconnected(IConnection connection)
        {
            OnDisconnected?.Invoke(connection);
            CallOnGraphChanged();
        }
    }
}