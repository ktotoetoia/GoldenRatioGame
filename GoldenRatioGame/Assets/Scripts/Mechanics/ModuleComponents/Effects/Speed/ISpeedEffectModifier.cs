using IM.Effects;
using IM.Common;

namespace IM.Modules
{
    public interface ISpeedEffectModifier : IEffectModifier
    {
        ISpeedModifier SpeedModifier { get; }
    }
}