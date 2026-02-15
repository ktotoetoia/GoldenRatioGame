using System.Collections.Generic;
using IM.Movement;
using TDS.Events;
using UnityEngine;

namespace IM.Visuals
{
    public class MovementDirectionFieldSetter : MonoBehaviour,IRequireModuleVisualObjectInitialization, IPoolObject
    {
        [SerializeField] private List<MovementDirectionEntry> _entries = new ();
        private readonly FieldSetterEntryResolver<MovementDirection> _resolver= new(); 
        private IValueStorageContainer _container;
        private IValueStorage<MovementDirection> _storage;
        
#if UNITY_EDITOR
        private void OnValidate() => _resolver.Resolve(_entries);
#endif
        
        private void Awake()=> _resolver.Resolve(_entries);

        public void OnModuleVisualObjectInitialized(IModuleVisualObject moduleVisualObject)
        {
            _container = moduleVisualObject.Owner.Extensions.GetExtension<IValueStorageContainer>();
        }
        
        public void OnRelease()
        {
            if(_container == null || _storage == null) return;

            _storage.Changed -= OnEnumChanged;
        }

        public void OnGet()
        {
            if(_container == null) return;
            
            _storage = _container.GetOrCreate<MovementDirection>();
            _storage.Changed += OnEnumChanged;
            OnEnumChanged(_storage.Value);
        }

        private void OnEnumChanged(MovementDirection direction)
        {
            const MovementDirection horizontalMask =
                MovementDirection.Left | MovementDirection.Right;

            if ((direction & horizontalMask) == 0)
                return;

            _resolver.OnValueChanged(direction);
        }
        
        private void OnDestroy() => OnRelease();
    }
}