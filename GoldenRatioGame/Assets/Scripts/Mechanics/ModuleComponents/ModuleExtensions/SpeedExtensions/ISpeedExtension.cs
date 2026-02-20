using IM.Values;

namespace IM.Modules
{
    public interface ISpeedExtension : IExtension
    {
        ISpeedModifier SpeedModifier { get; }
    }
}