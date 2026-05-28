using IM.Values;

namespace IM.Effects
{
    public interface IHealthEffectModifier : IEffectModifier
    {
        ICappedValue<float> Health { get; }
    }
}