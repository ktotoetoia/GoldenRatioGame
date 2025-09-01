using System.Collections.Generic;

namespace IM.Health
{
    public interface IFloatHealthComposition : IFloatHealth
    {
        IReadOnlyList<IFloatHealth> HealthComponents { get; }
    }
}