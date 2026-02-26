using System.Collections.Generic;
using IM.Base;
using IM.Values;
using IM.Events;
using UnityEngine;

namespace IM.Visuals
{
    public class HorizontalDirectionFieldSetter : MonoBehaviour,IRequireModuleVisualObjectInitialization, IPoolObject
    {
        [SerializeField] private string _tag = DirectionConstants.Focus;
        [SerializeField] private List<HorizontalDirectionEntry> _entries = new ();
        private readonly FieldSetterEntryResolver<Direction> _resolver= new(); 
        private IValueStorageContainer _container;
        private IValueStorage<Direction> _storage;
        
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
            
            _storage = _container.GetOrCreate<Direction>(_tag);
            _storage.Changed += OnEnumChanged;
            OnEnumChanged(_storage.Value);
        }

        private void OnEnumChanged(Direction direction)
        {
            const Direction horizontalMask =
                Direction.Left | Direction.Right;

            if ((direction & horizontalMask) == 0)
                return;

            _resolver.OnValueChanged(direction);
        }
        
        private void OnDestroy() => OnRelease();
    }
}