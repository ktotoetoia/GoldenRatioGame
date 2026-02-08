using System.Collections.Generic;
using IM.Modules;
using IM.Movement;
using UnityEngine;

namespace IM.Visuals
{
    public class MovementDirectionFieldSetter : MonoBehaviour,IRequireModuleVisualObjectInitialization, IPoolObject
    {
        [SerializeField] private List<MovementDirectionEntry> _entries = new ();
        private readonly FieldSetterEntryResolver<MovementDirection> _resolver= new(); 
        private IValueStateExtension<MovementDirection> _extension;
            
#if UNITY_EDITOR
        private void OnValidate() => _resolver.Resolve(_entries);
#endif
        
        private void Awake()=> _resolver.Resolve(_entries);

        public void OnModuleVisualObjectInitialized(IModuleVisualObject moduleVisualObject)
        {
            _extension = moduleVisualObject.Owner.Extensions.GetExtension<IValueStateExtension<MovementDirection>>();
        }
        
        public void OnRelease()
        {
            if(_extension == null) return;
            
            _extension.ValueChanged -= OnEnumChanged;
        }

        public void OnGet()
        {
            if(_extension == null) return;

            _extension.ValueChanged += OnEnumChanged;
            OnEnumChanged(_extension.Value);
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