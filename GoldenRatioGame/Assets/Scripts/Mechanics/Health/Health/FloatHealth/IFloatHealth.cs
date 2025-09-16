using IM.Values;

namespace IM.Health
{
    public interface IFloatHealth : IHealth
    {
        public ICappedValueReadOnly<float> Health { get; }
    }
}