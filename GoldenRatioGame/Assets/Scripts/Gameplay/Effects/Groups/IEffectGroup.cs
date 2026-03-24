using IM.LifeCycle;

namespace IM.Effects
{
    public interface IEffectGroup
    {
        ITypeRegistry<IEffectModifier> Modifiers { get; }
    }
}