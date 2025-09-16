using System.Collections.Generic;
using IM.Graphs;
using UnityEngine;

namespace IM.ModuleEditor
{
    public class PositionModuleGraph : IModuleGraphReadOnly
    {
        private readonly List<ModuleDecorator>  _modules = new();
        private readonly List<IModuleConnection> _connections =new ();
        
        public IReadOnlyList<INode> Nodes => Modules;
        public IReadOnlyList<IEdge> Edges => Connections;
        public IReadOnlyList<IModule> Modules => _modules;
        public IReadOnlyList<IModuleConnection> Connections => _connections;
        
        public Vector3 NewModulePosition { get; set; }
        public Vector3 NewModuleSize { get; set; } = Vector3.one;
        
        public PositionModuleGraph(IModuleGraphEvents moduleGraphEvents)
        {
            moduleGraphEvents.OnModuleAdded += x =>
            {
                _modules.Add(new ModuleDecorator(x){Size =NewModuleSize,Position = NewModulePosition});
            };
            
            moduleGraphEvents.OnModuleRemoved += x =>
            {
                _modules.RemoveAll(y => y.Module == x);
            };

            moduleGraphEvents.OnConnected += x =>
            {
                _connections.Add(x);
            };

            moduleGraphEvents.OnDisconnected += x =>
            {
                _connections.Remove(x);
            };
        }
    }
}