using IM.Values;

namespace IM.Modules
{
    public interface ISpeedExtension : IModuleExtension
    {
        ISpeedModifier SpeedModifier { get; }
    }
}