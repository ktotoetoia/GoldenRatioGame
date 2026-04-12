using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IM.Graphs
{
    public class ModuleGraphSnapshotDiffer : IEditorObserver<IModuleGraphReadOnly>
    {
        private readonly List<IModule> _prevModules = new();
        private readonly List<IConnection> _prevConnections = new();
        
        public Action<IModule> ModuleAdded { get; set; } = x => { };        
        public Action<IModule> ModuleRemoved { get; set; } = x => { };        
        public Action<IConnection> Connected { get; set; } = x => { };
        public Action<IConnection> Disconnected { get; set; } = x => { };
        
        public void OnSnapshotChanged(IModuleGraphReadOnly graph)
        {
            foreach (IConnection connection in _prevConnections.Except(graph.Connections))
            {
                Disconnected(connection);
            }
            
            foreach (IModule module in _prevModules.Except(graph.Modules))
            {
                ModuleRemoved(module);
            }

            foreach (IModule module in graph.Modules.Except(_prevModules))
            {
                ModuleAdded(module);
            }

            foreach (IConnection connection in graph.Connections.Except(_prevConnections))
            {
                Connected(connection);
            }
            
            _prevModules.Clear();
            _prevConnections.Clear();
            _prevModules.AddRange(graph.Modules);
            _prevConnections.AddRange(graph.Connections);
        }
    }
}