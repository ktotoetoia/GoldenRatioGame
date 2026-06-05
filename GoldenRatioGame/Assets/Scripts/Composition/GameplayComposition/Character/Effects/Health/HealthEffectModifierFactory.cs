using System;
using IM.Values;
using UnityEngine;

namespace IM.Effects
{
    [Serializable]
    public class HealthEffectModifierFactory : IRestorableEffectModifierFactory
    {
        [SerializeField] CappedValue<float> _cappedValue;
        
        public IEffectModifier Create(IEffectContext context)
        {
            return new HealthEffectModifier(new CappedValue<float>(_cappedValue.MinValue, _cappedValue.MaxValue, _cappedValue.Value));
        }

        public object Save(IEffectModifier modifier)
        {
            return ((HealthEffectModifier)modifier).Health;
        }

        public IEffectModifier Restore(object modifier, IEffectContext context)
        {
            return new HealthEffectModifier((CappedValue<float>)modifier);
        }
    }
}