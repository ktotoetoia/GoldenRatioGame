using System.Collections.Generic;

namespace IM.Effects
{
    public class EffectGroup : IEffectGroup
    {
        public IEnumerable<IEffectModifier> Modifiers { get; }
        
        public EffectGroup(IEnumerable<IEffectModifier> modifiers)
        {
            Modifiers = modifiers;
        }
    }
}