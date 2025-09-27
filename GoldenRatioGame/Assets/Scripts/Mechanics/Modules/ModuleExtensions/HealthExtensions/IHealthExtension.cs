using IM.Values;

namespace IM.Modules
{
    public interface IHealthExtension : IModuleExtension
    {
        ICappedValue<float> Health { get; }
    }
}