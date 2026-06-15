using System;
using IM.Effects;
using IM.SaveSystem;
using UnityEngine;

namespace IM.Modules
{
    public class EffectGroupExtensionSerializer : ComponentSerializer<EffectGroupExtension>
    {
        public override object CaptureState(EffectGroupExtension component)
        {
            return component.RestorableEffectGroupFactory.Save(component.EffectGroup);
        }
        
        public override void RestoreState(EffectGroupExtension component, object state, Func<string, GameObject> resolveDependency)
        {
            component.EffectGroup = component.RestorableEffectGroupFactory.Restore(state, new EffectContext(component.gameObject, component.gameObject));
        }
    }
}