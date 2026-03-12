using System.Collections.Generic;
using IM.Modules;

namespace IM.Effects
{
    public class EffectGroup : IEffectGroup
    {
        public ITypeRegistry<IEffectModifier> Modifiers { get; }
        
        public EffectGroup(IEnumerable<IEffectModifier> modifiers) : this(new TypeRegistry<IEffectModifier>(modifiers))
        {
            
        }
        
        public EffectGroup(ITypeRegistry<IEffectModifier> modifiers)
        {
            Modifiers = modifiers;
        }
    }
}