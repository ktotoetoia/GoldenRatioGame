using System.Collections.Generic;
using IM.Economy;

namespace IM.Health
{
    public interface IFloatHealthValuesGroup : IFloatHealth
    {
        IReadOnlyList<ICappedValueReadOnly<float>> Values { get; }
        
        void AddHealth(ICappedValue<float> healthBar);
        void RemoveHealth(ICappedValue<float> healthBar);
    }
}