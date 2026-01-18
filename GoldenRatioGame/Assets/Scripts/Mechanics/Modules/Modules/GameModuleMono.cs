using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.Items;
using UnityEngine;

namespace IM.Modules
{
    public class GameModuleMono : MonoBehaviour, IGameModule
    {
        [SerializeField] private Icon _icon;
        private readonly List<IPort> _ports =  new();
        private IModuleExtensions _extensions;

        public IIcon Icon => _icon;
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set;}
        
        public IEnumerable<IEdge> Edges => _ports.Where(x => x.IsConnected).Select(x => x.Connection).ToList();
        public IEnumerable<IPort> Ports => _ports;
        public IModuleExtensions Extensions => _extensions ??= new GameObjectModuleExtensions(gameObject);

        public void AddPort(IPort port)
        {
            _ports.Add(port);
        }
    }
}