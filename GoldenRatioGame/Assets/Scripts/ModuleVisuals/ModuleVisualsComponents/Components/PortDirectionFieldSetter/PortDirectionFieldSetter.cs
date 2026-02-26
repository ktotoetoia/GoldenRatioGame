using System.Collections.Generic;
using IM.Base;
using IM.Modules;
using IM.Events;
using UnityEngine;

namespace IM.Visuals
{
    public class PortDirectionFieldSetter : MonoBehaviour,IRequireModuleVisualObjectInitialization, IPoolObject
    {
        [SerializeField] private List<PortDirectionEntry> _entries = new ();
        private readonly FieldSetterEntryResolver<PortDirection> _resolver= new(); 
        private IValueStorageContainer _container;
        private IValueStorage<PortDirection> _storage;
        
        
#if UNITY_EDITOR
        private void OnValidate() => _resolver.Resolve(_entries);
#endif
        
        private void Awake()=> _resolver.Resolve(_entries);

        public void OnModuleVisualObjectInitialized(IModuleVisualObject moduleVisualObject)
        {
            _container = moduleVisualObject.Owner.Extensions.Get<IValueStorageContainer>();
        }
        
        public void OnRelease()
        {
            if(_container == null || _storage == null) return;

            _storage.Changed -= OnEnumChanged;
        }

        public void OnGet()
        {
            if(_container == null) return;
            
            _storage = _container.GetOrCreate<PortDirection>();
            _storage.Changed += OnEnumChanged;
            OnEnumChanged(_storage.Value);
        }
        
        private void OnEnumChanged(PortDirection direction)
        {
            _resolver.OnValueChanged(direction);
        }
        
        private void OnDestroy() => OnRelease();
    }
}