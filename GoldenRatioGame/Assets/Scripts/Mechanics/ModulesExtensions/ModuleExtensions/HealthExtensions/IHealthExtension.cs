using IM.Values;

namespace IM.Modules
{
    public interface IHealthExtension : IExtension
    {
        ICappedValue<float> Health { get; }
    }
}