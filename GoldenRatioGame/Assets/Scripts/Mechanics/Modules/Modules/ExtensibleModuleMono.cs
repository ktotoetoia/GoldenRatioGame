using System;
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
        private readonly List<IPort> _ports =  new();
        private IExtensionController _extensions;
        private IIconDrawer  _iconDrawer;
        private ModuleState _state;
        private IIconDrawer IconDrawer => _iconDrawer ??= GetComponent<IIconDrawer>();
        
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set;}
        public IIcon Icon => IconDrawer.Icon;

        public ModuleState State
        {
            get => _state;
            set
            {
                if(_state == value) return;
                
                _state = value;
                IconDrawer.IsDrawing = value == ModuleState.OnGround;
            }
        }
        
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