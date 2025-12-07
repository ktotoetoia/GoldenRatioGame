using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using IM.Graphs;

namespace IM.Visuals
{
    public class SingleIconVisualModule : IVisualModule
    {
        private readonly List<IVisualPort> _ports = new();
        
        public IEnumerable<IEdge> Edges => _ports.Where(x => x.IsConnected).Select(x => x.Connection);
        IEnumerable<IPort> IModule.Ports => _ports;
        public IEnumerable<IVisualPort> Ports => _ports;
        public Sprite Icon { get; }
        public IHierarchyTransform Transform { get; }

        public SingleIconVisualModule(Sprite sprite) : this(new Transform(), sprite)
        {
            
        }
        
        public SingleIconVisualModule(IHierarchyTransform transform,Sprite sprite)
        {
            Transform = transform;
            Icon = sprite;
        }
        
        public void AddPort(IVisualPort port)
        {
            _ports.Add(port);
        }
    }
}