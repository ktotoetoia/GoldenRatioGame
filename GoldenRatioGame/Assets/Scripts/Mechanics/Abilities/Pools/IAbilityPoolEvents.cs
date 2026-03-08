using System;

namespace IM.Abilities
{
    public interface IAbilityPoolEvents
    {
        event Action<IAbilityReadOnly> AbilityAdded;
        event Action<IAbilityReadOnly> AbilityRemoved;
    }
}