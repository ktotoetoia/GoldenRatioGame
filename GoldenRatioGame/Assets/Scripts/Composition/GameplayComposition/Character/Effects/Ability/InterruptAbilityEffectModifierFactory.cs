using System;
using UnityEngine;

namespace IM.Effects
{
    [Serializable]
    public class InterruptAbilityEffectModifierFactory : IEffectModifierFactory
    {
        [SerializeField] private bool _interrupts = true;

        public IEffectModifier Create(IEffectContext param1) =>  new InterruptAbilityEffectModifier(_interrupts);
    }
}