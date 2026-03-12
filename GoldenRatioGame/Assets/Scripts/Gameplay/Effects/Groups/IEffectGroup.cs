using IM.Modules;

namespace IM.Effects
{
    public interface IEffectGroup
    {
        ITypeRegistry<IEffectModifier> Modifiers { get; }
    }
}