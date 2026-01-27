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
        private IExtensionProvider _extensions;
        private IIconDrawer  _iconDrawer;
        private ModuleState _state;
        private IIconDrawer IconDrawer => _iconDrawer ??= GetComponent<IIconDrawer>();
        
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set;}
        
        public IIcon Icon => IconDrawer.Icon;
        public List<IPort> PortsList { get; } = new(); 
        public IStorageCell Cell { get; set; }
        public IEnumerable<IEdge> Edges => PortsList.Where(x => x.IsConnected).Select(x => x.Connection).ToList();
        public IEnumerable<IPort> Ports => PortsList;
        public IExtensionProvider Extensions => _extensions ??= new GameObjectExtensionProvider(gameObject);
        public ModuleState State
        {
            get => _state;
            set => IconDrawer.IsDrawing = (_state = value) == ModuleState.Show;
        }
    }
}