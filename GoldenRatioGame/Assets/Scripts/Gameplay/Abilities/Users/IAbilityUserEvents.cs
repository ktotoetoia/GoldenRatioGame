using System;

namespace IM.Abilities
{
    public interface IAbilityUserEvents
    {
        event Action<IAbilityReadOnly> AbilityStarted;
        event Action<IAbilityReadOnly> AbilityFinished;
    }
}