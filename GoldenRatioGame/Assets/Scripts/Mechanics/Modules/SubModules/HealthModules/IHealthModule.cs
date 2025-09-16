using IM.Values;

namespace IM.Modules
{
    public interface IHealthModule
    {
        ICappedValue<float> Health { get; }
    }
}