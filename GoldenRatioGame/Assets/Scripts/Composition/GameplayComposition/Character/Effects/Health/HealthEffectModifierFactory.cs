using IM.Values;
using UnityEngine;

namespace IM.Effects
{
    [CreateAssetMenu(menuName = "Effects/Modifiers/Health Effect Modifier")]
    public class HealthEffectModifierFactory : EffectModifierFactory
    {
        [SerializeField] CappedValue<float> _cappedValue;
        
        public override IEffectModifier Create(IEffectContext context)
        {
            return new HealthEffectModifier(new CappedValue<float>(_cappedValue.MinValue, _cappedValue.MaxValue, _cappedValue.Value));
        }
    }
}