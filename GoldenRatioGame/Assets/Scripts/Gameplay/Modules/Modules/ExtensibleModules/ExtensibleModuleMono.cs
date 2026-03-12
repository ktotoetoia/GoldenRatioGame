using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.Items;
using IM.Storages;
using UnityEngine;

namespace IM.Modules
{
    [RequireComponent(typeof(SpriteRendererIconDrawer))]
    public class ExtensibleModuleMono : MonoBehaviour, IExtensibleModule
    {
        [SerializeField] private PortInitializationBase _portInitialization;
        private readonly List<IPort> _ports =  new();
        private ITypeRegistry<IExtension> _extensions;
        private IIconDrawer _iconDrawer;
        private ModuleState _state;
        
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        public IIcon Icon => _iconDrawer.Icon;
        public IStorageCell Cell { get; set; }
        public IEnumerable<IEdge> Edges => _ports.Where(x => x.IsConnected).Select(x => x.Connection);
        public IEnumerable<IPort> Ports => _ports;
        public ITypeRegistry<IExtension> Extensions => _extensions ??= new TypeRegistry<IExtension>(gameObject.GetComponents<IExtension>());

        public ModuleState ModuleState
        {
            get => _state;
            set => _iconDrawer.IsDrawing = (_state = value) == ModuleState.Show;
        }

        private void Awake()
        {
            _iconDrawer = GetComponent<IIconDrawer>();
            ModuleState = ModuleState.Show;
            
            _portInitialization.Initialize(_ports, this);
        }
    }
}