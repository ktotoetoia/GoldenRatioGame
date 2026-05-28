using UnityEngine;

namespace IM.Effects
{
    public abstract class EffectModifierFactory : ScriptableObject, IEffectModifierFactory
    {
        public abstract IEffectModifier Create(IEffectContext context);
    }
}