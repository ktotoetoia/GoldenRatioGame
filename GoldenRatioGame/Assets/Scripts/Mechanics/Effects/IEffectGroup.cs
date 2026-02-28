using System.Collections.Generic;

namespace IM.Effects
{
    public interface IEffectGroup
    {
        IEnumerable<IEffectModifier> Modifiers { get; }
    }
}