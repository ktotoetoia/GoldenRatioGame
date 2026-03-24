using IM.Effects;
using IM.Values;

namespace IM.Modules
{
    public interface ISpeedEffectModifier : IEffectModifier
    {
        ISpeedModifier SpeedModifier { get; }
    }
}