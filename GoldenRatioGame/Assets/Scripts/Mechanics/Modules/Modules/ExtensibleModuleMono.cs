using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.Items;
using IM.Storages;
using IM.Visuals;
using UnityEngine;

namespace IM.Modules
{
    [RequireComponent(typeof(SpriteRendererIconDrawer))]
    public class ExtensibleModuleMono : MonoBehaviour, IExtensibleModule, IPortInfoProvider
    {
        [SerializeField] private List<PortInfo> _portsInfos;
        private readonly List<IPort> _ports =  new();
        private readonly Dictionary<IPort, PortInfo>  _portInfos = new();
        private IExtensionProvider _extensions;
        private IIconDrawer _iconDrawer;
        private ModuleState _state;
        
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }

        public IIcon Icon => _iconDrawer.Icon;
        public IStorageCell Cell { get; set; }
        public IEnumerable<IEdge> Edges => _ports.Where(x => x.IsConnected).Select(x => x.Connection);
        public IEnumerable<IPort> Ports => _ports;
        public IExtensionProvider Extensions => _extensions ??= new GameObjectExtensionProvider(gameObject);
        public IReadOnlyDictionary<IPort, PortInfo> PortsInfos => _portInfos;

        public ModuleState State
        {
            get => _state;
            set => _iconDrawer.IsDrawing = (_state = value) == ModuleState.Show;
        }

        private void Awake()
        {
            _iconDrawer = GetComponent<IIconDrawer>();
            State = ModuleState.Show;
            
            foreach (PortInfo portInfo in _portsInfos)
            {
                IPort port = portInfo.Tag == null ? new Port(this) : new TaggedPort(this, portInfo.Tag);
                
                _ports.Add(port);
                _portInfos.Add(port, portInfo);
            }
        }
    }
}