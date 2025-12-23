using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class GameModuleMono : MonoBehaviour, IGameModule
    {
        private readonly List<IPort> _ports =  new();
        private IModuleExtensions _extensions;
        
        public IEnumerable<IEdge> Edges => _ports.Where(x => x.IsConnected).Select(x => x.Connection).ToList();
        public IEnumerable<IPort> Ports => _ports;
        public IModuleExtensions Extensions => _extensions ??= new ModuleExtensions(gameObject);

        public void AddPort(IPort port)
        {
            _ports.Add(port);
        }
    }
}