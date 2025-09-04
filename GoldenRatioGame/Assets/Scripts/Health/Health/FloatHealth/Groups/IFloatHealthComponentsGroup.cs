using System.Collections.Generic;

namespace IM.Health
{
    public interface IFloatHealthComponentsGroup : IFloatHealth
    {
        IReadOnlyList<IFloatHealth> Components { get; }
        
        void AddHealth(IFloatHealth healthComponent);
        void RemoveHealth(IFloatHealth healthComponent);
    }
}