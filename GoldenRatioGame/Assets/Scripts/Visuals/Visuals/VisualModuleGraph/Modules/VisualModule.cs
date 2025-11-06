using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using IM.Graphs;
using IM.Modules;

namespace IM.ModuleGraph
{
    public class VisualModule : IVisualModule
    {
        private readonly List<IVisualPort> _ports = new();
        
        public IEnumerable<IEdge> Edges => _ports.Where(x => x.IsConnected).Select(x => x.Connection);
        
        IEnumerable<IPort> IModule.Ports => _ports;
        public IEnumerable<IVisualPort> Ports => _ports;

        public Vector3 Position { get; set; }
        
        public VisualModule(Vector3 position = new())
        {
            Position = position;
        }
        
        public void AddPort(IVisualPort port)
        {
            _ports.Add(port);
        }
    }
}