using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using IM.Graphs;

namespace IM.Visuals
{
    public class VisualModule : IVisualModule
    {
        private readonly List<IVisualPort> _ports = new();
        
        public IEnumerable<IEdge> Edges => _ports.Where(x => x.IsConnected).Select(x => x.Connection);
        IEnumerable<IPort> IModule.Ports => _ports;
        public IEnumerable<IVisualPort> Ports => _ports;
        public Sprite Sprite { get; }
        public ITransform Transform { get; }

        public VisualModule(Sprite sprite) : this(new Transform(), sprite)
        {
            
        }
        
        public VisualModule(ITransform transform,Sprite sprite)
        {
            Transform = transform;
            Sprite = sprite;
        }
        
        public void AddPort(IVisualPort port)
        {
            _ports.Add(port);
        }
    }
}