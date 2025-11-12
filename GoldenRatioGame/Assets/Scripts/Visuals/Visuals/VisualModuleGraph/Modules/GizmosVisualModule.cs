using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using IM.Graphs;

namespace IM.Visuals
{
    public class GizmosVisualModule : IVisualModule
    {
        private readonly List<IVisualPort> _ports = new();
        
        public IEnumerable<IEdge> Edges => _ports.Where(x => x.IsConnected).Select(x => x.Connection);
        public Sprite Sprite { get; set; }
        IEnumerable<IPort> IModule.Ports => _ports;
        public IEnumerable<IVisualPort> Ports => _ports;
        public ITransform Transform { get; }
        
        public GizmosVisualModule(ITransform transform)
        {
            Transform = transform;
        }
        
        public void AddPort(IVisualPort port)
        {
            _ports.Add(port);
        }
    }
}