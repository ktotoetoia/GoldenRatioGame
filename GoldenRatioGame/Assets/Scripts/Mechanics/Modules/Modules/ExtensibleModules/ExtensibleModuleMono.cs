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
        private readonly List<IPort> _ports =  new();
        private readonly Dictionary<IPort, PortInfo>  _portInfos = new();
        private IExtensionProvider _extensions;
        private IIconDrawer _iconDrawer;
        private ModuleState _state;
        
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [SerializeField] private List<PortInfo> _portsInfos;

        public IIcon Icon => _iconDrawer.Icon;
        public IStorageCell Cell { get; set; }
        public IEnumerable<IEdge> Edges => _ports.Where(x => x.IsConnected).Select(x => x.Connection);
        public IEnumerable<IPort> Ports => _ports;
        public IExtensionProvider Extensions => _extensions ??= new GameObjectExtensionProvider(gameObject);
        public IReadOnlyDictionary<IPort, PortInfo> PortsInfos => _portInfos;

        public ModuleState ModuleState
        {
            get => _state;
            set => _iconDrawer.IsDrawing = (_state = value) == ModuleState.Show;
        }

        private void Awake()
        {
            _iconDrawer = GetComponent<IIconDrawer>();
            ModuleState = ModuleState.Show;
            
            foreach (PortInfo portInfo in _portsInfos)
            {
                ITag tag = portInfo.Tag ?? new FreeTag();
                IPort port = portInfo.Direction == HorizontalDirection.None ?new TaggedPort(this, tag): new EnumChangingTaggedPort<HorizontalDirection>(this,tag,portInfo.Direction); 
                
                _ports.Add(port);
                _portInfos.Add(port, portInfo);
            }
        }
    }
}