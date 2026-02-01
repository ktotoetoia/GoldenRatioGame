using System.Collections.Generic;
using IM.Modules;
using UnityEngine;

namespace IM.Visuals
{
    public class HorizontalDirectionSetter : MonoBehaviour,IRequireModuleVisualObjectInitialization
    {
        private readonly List<IHorizontalDirectionDependant> _dependants = new();
        private IEnumStateExtension<HorizontalDirection> _extension;
        
        private void Awake()
        {
            GetComponents(_dependants);
        }

        public void OnModuleVisualObjectInitialized(IModuleVisualObject moduleVisualObject)
        {
            _extension = moduleVisualObject.Owner.Extensions.GetExtension<IEnumStateExtension<HorizontalDirection>>();
            _extension.ValueChanged += NotifyChange;
            NotifyChange(_extension.Value);
        }
        
        private void NotifyChange(HorizontalDirection direction)
        {
            _dependants.ForEach(x => x.OnDirectionChanged(direction));
        }
        
        private void OnDisable()
        {
            if (_extension != null) _extension.ValueChanged -= NotifyChange;
        }
    }
}