using IM.Economy;

namespace IM.Modules
{
    public interface IHealthModule
    {
        ICappedValue<float> Health { get; }
    }
}