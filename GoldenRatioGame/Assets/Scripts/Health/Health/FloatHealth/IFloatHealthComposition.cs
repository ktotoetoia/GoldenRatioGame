using System.Collections.Generic;

namespace IM.Health
{
    public interface IFloatHealthComposition : IFloatHealth
    {
        IReadOnlyList<IFloatHealth> HealthComponents { get; }
        
        public void AddHealth(IFloatHealth healthComponent);
        public void RemoveHealth(IFloatHealth healthComponent);
    }
}