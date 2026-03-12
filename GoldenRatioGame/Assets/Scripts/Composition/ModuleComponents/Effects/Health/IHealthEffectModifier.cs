using IM.Effects;
using IM.Common;

namespace IM.Modules
{
    public interface IHealthEffectModifier : IEffectModifier
    {
        ICappedValue<float> Health { get; }
    }
}