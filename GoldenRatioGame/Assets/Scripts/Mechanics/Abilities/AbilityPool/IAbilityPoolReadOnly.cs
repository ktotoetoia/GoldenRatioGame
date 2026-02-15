using System.Collections.Generic;

namespace IM.Abilities
{
    public interface IAbilityPoolReadOnly
    {
        IReadOnlyCollection<IAbility> Abilities { get; }
    }
}