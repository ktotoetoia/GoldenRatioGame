using System.Collections.Generic;

namespace IM.Abilities
{
    public interface IAbilitiesPool
    {
        IEnumerable<IAbility> Abilities { get; }
    }
}