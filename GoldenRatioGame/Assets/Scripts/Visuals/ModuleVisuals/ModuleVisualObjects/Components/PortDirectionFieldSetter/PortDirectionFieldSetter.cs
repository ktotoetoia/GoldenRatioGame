using System.Collections.Generic;
using IM.Modules;
using UnityEngine;

namespace IM.Visuals
{
    public class PortDirectionFieldSetter : MonoBehaviour,IRequireModuleVisualObjectInitialization, IPoolObject
    {
        [SerializeField] private List<PortDirectionEntry> _entries = new ();
        private readonly FieldSetterEntryResolver<PortDirection> _resolver= new(); 
        private IValueStateExtension<PortDirection> _extension;
        
        
#if UNITY_EDITOR
        private void OnValidate() => _resolver.Resolve(_entries);
#endif
        
        private void Awake()=> _resolver.Resolve(_entries);

        public void OnModuleVisualObjectInitialized(IModuleVisualObject moduleVisualObject)
        {
            _extension = moduleVisualObject.Owner.Extensions.GetExtension<IValueStateExtension<PortDirection>>();
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
        
        private void OnEnumChanged(PortDirection direction)
        {
            _resolver.OnValueChanged(direction);
        }
        
        private void OnDestroy() => OnRelease();
    }
}