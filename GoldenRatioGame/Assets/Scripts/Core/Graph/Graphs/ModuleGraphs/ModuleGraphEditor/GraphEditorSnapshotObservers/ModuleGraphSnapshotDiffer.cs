using System;
using System.Collections.Generic;
using System.Linq;

namespace IM.Graphs
{
    public class ModuleGraphSnapshotDiffer : IModuleGraphSnapshotObserver
    {
        private readonly List<IModule> _prevModules = new();
        private readonly List<IConnection> _prevConnections = new();
        
        public Action<IModule> OnModuleAdded { get; set; } = x => { };        
        public Action<IModule> OnModuleRemoved { get; set; } = x => { };        
        public Action<IConnection> OnConnected { get; set; } = x => { };
        public Action<IConnection> OnDisconnected { get; set; } = x => { };
        
        public void OnGraphUpdated(IModuleGraphReadOnly graph)
        {
            foreach (IModule module in graph.Modules.Except(_prevModules))
            {
                OnModuleAdded(module);
            }

            foreach (IModule module in _prevModules.Except(graph.Modules))
            {
                OnModuleRemoved(module);
            }

            foreach (IConnection connection in graph.Connections.Except(_prevConnections))
            {
                OnConnected(connection);
            }
            
            foreach (IConnection connection in _prevConnections.Except(graph.Connections))
            {
                OnDisconnected(connection);
            }
            
            _prevModules.Clear();
            _prevConnections.Clear();
            _prevModules.AddRange(graph.Modules);
            _prevConnections.AddRange(graph.Connections);
        }
    }
}