using IM.Economy;

namespace IM.Health
{
    public interface IFloatHealth : IDamageable
    {
        public ICappedValueReadOnly<float> Health { get; }
    }
}