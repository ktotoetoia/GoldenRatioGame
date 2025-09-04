using System.Collections.Generic;
using IM.Economy;

namespace IM.Health
{
    public interface IFloatHealthValueGroup : IFloatHealth
    {
        IReadOnlyList<ICappedValueReadOnly<float>> HealthBars { get; }
        
        void AddHealth(ICappedValue<float> healthBar);
        void RemoveHealth(ICappedValue<float> healthBar);
    }
}