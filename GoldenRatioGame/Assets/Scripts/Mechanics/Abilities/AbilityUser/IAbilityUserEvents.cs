using System;

namespace IM.Abilities
{
    public interface IAbilityUserEvents
    {
        event Action<IAbilityReadOnly, AbilityUseContext> OnAbilityUsed;
    }
}