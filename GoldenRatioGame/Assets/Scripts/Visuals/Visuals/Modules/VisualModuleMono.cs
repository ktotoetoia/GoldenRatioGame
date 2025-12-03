using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using UnityEngine;

namespace IM.Visuals
{
    public class VisualModuleMono : MonoBehaviour, IVisualModule
    {
        private readonly List<IVisualPort> _ports = new();

        public Sprite Icon { get; set; }
        public IEnumerable<IEdge> Edges => _ports.Where(x => x.IsConnected).Select(x => x.Connection);
        public IEnumerable<IVisualPort> Ports => _ports;
        IEnumerable<IPort> IModule.Ports => _ports;
        public ITransform Transform { get; } = new Transform();

        public void AddPort(IVisualPort port)
        {
            _ports.Add(port);
        }
    }
}