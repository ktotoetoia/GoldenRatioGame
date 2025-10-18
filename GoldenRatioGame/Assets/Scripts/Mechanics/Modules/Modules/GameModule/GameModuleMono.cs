using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class GameModuleMono : MonoBehaviour, IGameModule
    {
        [SerializeField] private int _portCount;
        private readonly List<IPort>  _ports = new();
        
        public IEnumerable<IEdge> Edges => _ports.Where(x => x.IsConnected).Select(x => x.Connection).ToList();
        public IEnumerable<IPort> Ports => _ports;
        public IModuleLayout Layout { get; private set; }
        public IModuleExtensions Extensions { get; private set; }
        
        private void Awake()
        {
            Extensions = GetComponent<IModuleExtensions>();
            
            for (int i = 0; i < _portCount; i++)
                _ports.Add(new Port(this));
            
            Layout = new DefaultModuleLayout(_ports);
        }
    }
}