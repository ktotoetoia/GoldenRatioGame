using System.Collections.Generic;
using UnityEngine;

namespace IM.Effects
{
    public abstract class EffectGroupFactory : ScriptableObject, IEffectGroupFactory
    {
        public abstract IEffectGroup Create(IEffectContext context);
        public abstract IEffectGroup Create(IEnumerable<IEffectModifier> effectModifiers,IEffectContext context);
    }
}