using System;
using IM.Modules;
using IM.Values;
using UnityEngine;

namespace IM.Effects
{
    [Serializable]
    public class SpeedEffectModifierFactory : IRestorableEffectModifierFactory
    {
        [field:SerializeField] public float Add { get; set; }
        [field: SerializeField] public float Multiplication { get; set; }
        
        public IEffectModifier Create(IEffectContext context)
        {
            return new SpeedEffectModifier(new SpeedModifier(Add,Multiplication));
        }

        public object Save(IEffectModifier modifier)
        {
            return ((SpeedEffectModifier)modifier).SpeedModifier;
        }

        public IEffectModifier Restore(object modifier, IEffectContext context)
        {
            return new SpeedEffectModifier((SpeedModifier)modifier);
        }
    }
}