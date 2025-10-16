using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class GameModuleComponent : MonoBehaviour, IGameModule
    {
        [SerializeField] private int _portCount;
        private List<IModulePort>  _ports = new();
        
        public IEnumerable<IEdge> Edges => _ports.Where(x => x.IsConnected).Select(x => x.Connection).ToList();
        public IEnumerable<IModulePort> Ports =>_ports;
        public IModuleLayout Layout { get; private set; }
        public IModuleContextExtensions Extensions { get; private set; }

        private void Awake()
        {
            Extensions = new ModuleContextExtensions(GetComponents<IModuleExtension>());
            
            for (int i = 0; i < _portCount; i++)
                _ports.Add(new ModulePort(this));
        }
    }
}