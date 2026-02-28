using IM.Effects;
using IM.Values;

namespace IM.Modules
{
    public interface IHealthEffectModifier : IEffectModifier
    {
        ICappedValue<float> Health { get; }
    }
}