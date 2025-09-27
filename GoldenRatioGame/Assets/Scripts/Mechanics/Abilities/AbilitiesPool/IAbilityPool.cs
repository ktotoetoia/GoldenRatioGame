using System.Collections.Generic;

namespace IM.Abilities
{
    public interface IAbilityPool
    {
        IEnumerable<IAbility> Abilities { get; }
    }
}