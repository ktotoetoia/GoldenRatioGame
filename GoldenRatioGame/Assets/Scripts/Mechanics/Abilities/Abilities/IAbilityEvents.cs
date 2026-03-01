using System;

namespace IM.Abilities
{
    public interface IAbilityEvents
    {
        event Action<AbilityUseContext> AbilityStarted;
        event Action<AbilityUseContext> AbilityFinished;
    }
}