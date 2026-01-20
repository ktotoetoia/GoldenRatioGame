using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.Items;
using IM.Storages;
using UnityEngine;

namespace IM.Modules
{
    public class ExtensibleModuleMono : MonoBehaviour, IExtensibleModule
    {
        [SerializeField] private Icon _icon;
        private readonly List<IPort> _ports =  new();
        private IExtensionController _extensions;

        public IIcon Icon => _icon;
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set;}
        public IStorageCell Cell { get; set; }
        
        public IEnumerable<IEdge> Edges => _ports.Where(x => x.IsConnected).Select(x => x.Connection).ToList();
        public IEnumerable<IPort> Ports => _ports;
        public IExtensionController Extensions => _extensions ??= new GameObjectExtensionController(gameObject);

        public void AddPort(IPort port)
        {
            _ports.Add(port);
        }

    }
}