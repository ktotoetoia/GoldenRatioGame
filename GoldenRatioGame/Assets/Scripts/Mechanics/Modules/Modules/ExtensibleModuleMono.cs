using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.Items;
using IM.Storages;
using UnityEngine;

namespace IM.Modules
{
    //todo
    //add state machine with 3 states: module on ground, module in storage, module as a part of a graph
    [RequireComponent(typeof(SpriteRendererIconDrawer))]
    public class ExtensibleModuleMono : MonoBehaviour, IExtensibleModule
    {
        private readonly List<IPort> _ports =  new();
        private IExtensionController _extensions;
        private IStorageCell _cell;
        private IIconDrawer  _iconDrawer;

        private IIconDrawer IconDrawer => _iconDrawer ??= GetComponent<IIconDrawer>();
        
        public IIcon Icon => IconDrawer.Icon;
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set;}

        public IStorageCell Cell
        {
            get => _cell;
            set
            {
                _cell = value;
                IconDrawer.IsDrawing = value == null;
            }
        }
        public IEnumerable<IEdge> Edges => _ports.Where(x => x.IsConnected).Select(x => x.Connection).ToList();
        public IEnumerable<IPort> Ports => _ports;
        public IExtensionController Extensions => _extensions ??= new GameObjectExtensionController(gameObject);

        public void AddPort(IPort port)
        {
            _ports.Add(port);
        }
    }
}