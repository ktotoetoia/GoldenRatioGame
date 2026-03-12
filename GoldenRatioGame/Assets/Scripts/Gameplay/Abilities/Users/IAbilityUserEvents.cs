using System;

namespace IM.Abilities
{
    public interface IAbilityUserEvents
    {
        event Action<IAbilityReadOnly> OnAbilityStarted;
        event Action<IAbilityReadOnly> OnAbilityFinished;
    }
}