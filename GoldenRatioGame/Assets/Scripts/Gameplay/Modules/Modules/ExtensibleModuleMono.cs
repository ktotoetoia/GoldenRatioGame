using System;
using IM.Items;
using IM.LifeCycle;
using IM.Storages;
using UnityEngine;

namespace IM.Modules
{
    public class ExtensibleModuleMono : MonoBehaviour, IExtensibleItem
    {
        [SerializeField] private PortFactoryBase portFactory;
        private ITypeRegistry<IExtension> _extensions;
        private IIconDrawer _iconDrawer;
        private ItemState _state;
        
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        public event Action<IEntity> Destroyed;
        public GameObject GameObject => gameObject;
        public IIcon Icon => _iconDrawer.Icon;
        public IStorageCell Cell { get; set; }
        public ITypeRegistry<IExtension> Extensions => _extensions ??= new TypeRegistry<IExtension>(gameObject.GetComponents<IExtension>());
        public IPortFactory PortFactory => portFactory;

        public ItemState ItemState
        {
            get => _state;
            set => _iconDrawer.IsDrawing = (_state = value) == ItemState.Show;
        }

        private void Awake()
        {
            _iconDrawer = GetComponent<IIconDrawer>();
            ItemState = ItemState.Show;
        }
        
        public void Destroy()
        {
            Destroy(gameObject);
        }
        
        private void OnDestroy()
        {
            Destroyed?.Invoke(this);
            Destroyed = null;
        }
    }
}