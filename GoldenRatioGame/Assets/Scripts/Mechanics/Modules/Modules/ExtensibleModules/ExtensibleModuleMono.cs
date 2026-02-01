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
        [SerializeField] private PortInitializationBase _portInitialization;
        private readonly List<IPort> _ports =  new();
        private readonly Dictionary<IPort, IPortInfo>  _portInfos = new();
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
        public IReadOnlyDictionary<IPort, IPortInfo> PortsInfos => _portInfos;

        public ModuleState ModuleState
        {
            get => _state;
            set => _iconDrawer.IsDrawing = (_state = value) == ModuleState.Show;
        }

        private void Awake()
        {
            _iconDrawer = GetComponent<IIconDrawer>();
            ModuleState = ModuleState.Show;
            
            _portInitialization.Initialize(_ports, _portInfos, this);
        }
    }
}