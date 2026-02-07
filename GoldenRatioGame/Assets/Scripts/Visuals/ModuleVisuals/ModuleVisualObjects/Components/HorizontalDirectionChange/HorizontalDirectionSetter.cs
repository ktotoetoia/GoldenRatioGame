using System.Collections.Generic;
using IM.Modules;
using UnityEngine;

namespace IM.Visuals
{
    public class HorizontalDirectionSetter : MonoBehaviour,IRequireModuleVisualObjectInitialization, IPoolObject
    {
        private readonly List<IHorizontalDirectionDependant> _dependants = new();
        private IEnumStateExtension<HorizontalDirection> _extension;
        
        private void Awake()
        {
            GetComponentsInChildren(_dependants);
        }
        
        public void OnModuleVisualObjectInitialized(IModuleVisualObject moduleVisualObject)
        {
            _extension = moduleVisualObject.Owner.Extensions.GetExtension<IEnumStateExtension<HorizontalDirection>>();
        }
        
        private void NotifyChange(HorizontalDirection direction)
        {
            _dependants.ForEach(x => x.OnDirectionChanged(direction));
        }
        
        public void OnRelease()
        {
            if(_extension == null) return;
            
            _extension.ValueChanged -= NotifyChange;
        }

        public void OnGet()
        {
            if(_extension == null) return;
            
            _extension.ValueChanged += NotifyChange;
            NotifyChange(_extension.Value);
        }
        
        private void OnDestroy() => OnRelease();
    }
}