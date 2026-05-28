using IM.Modules;
using IM.Values;
using UnityEngine;

namespace IM.Effects
{
    [CreateAssetMenu(menuName = "Effects/Modifiers/Speed Effect Modifier")]
    public class SpeedEffectModifierFactory : EffectModifierFactory
    {
        [field:SerializeField] public float Add { get; set; }
        [field: SerializeField] public float Multiplication { get; set; }
        
        public override IEffectModifier Create(IEffectContext context)
        {
            return new SpeedEffectModifier(new SpeedModifier(Add,Multiplication));
        }
    }
}