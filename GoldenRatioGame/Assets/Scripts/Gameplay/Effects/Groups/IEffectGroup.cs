using IM.Common;

namespace IM.Effects
{
    public interface IEffectGroup
    {
        ITypeRegistry<IEffectModifier> Modifiers { get; }
    }
}