using System;
using IM.Effects;
using IM.SaveSystem;
using IM.Values;
using UnityEngine;

namespace IM.Modules
{
    public class HealthEffectModuleComponentSerializer : ComponentSerializer<HealthEffectModifierMono>
    {
        public override object CaptureState(HealthEffectModifierMono component)
        {
            return new CappedValue<float>(component.Health.MinValue, component.Health.MaxValue,component.Health.Value);
        }
        
        public override void RestoreState(HealthEffectModifierMono component, object state, Func<string, GameObject> resolveDependency)
        {
            CappedValue<float> value = (CappedValue<float>)state;
            component.Health.MinValue = value.MinValue;
            component.Health.MaxValue = value.MaxValue;
            component.Health.Value = value.Value;
        }
    }
}