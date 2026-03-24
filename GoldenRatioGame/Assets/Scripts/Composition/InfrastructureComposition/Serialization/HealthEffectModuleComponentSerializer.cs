using System;
using IM.SaveSystem;
using IM.Values;
using UnityEngine;

namespace IM.Modules
{
    public class HealthEffectModuleComponentSerializer : ComponentSerializer<HealthEffectModifier>
    {
        public override object CaptureState(HealthEffectModifier component)
        {
            return new CappedValue<float>(component.Health.MinValue, component.Health.MaxValue,component.Health.Value);
        }
        
        public override void RestoreState(HealthEffectModifier component, object state, Func<string, GameObject> resolveDependency)
        {
            CappedValue<float> value = (CappedValue<float>)state;
            component.Health.MinValue = value.MinValue;
            component.Health.MaxValue = value.MaxValue;
            component.Health.Value = value.Value;
        }
    }
}