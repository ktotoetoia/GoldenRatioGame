using System;

namespace IM.Abilities
{
    public interface IAbilityUserEvents
    {
        event Action<IAbility, AbilityUseContext> OnAbilityUsed;
    }
}