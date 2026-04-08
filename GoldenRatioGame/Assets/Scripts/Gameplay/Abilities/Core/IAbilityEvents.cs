using System;
using IM.Values;

namespace IM.Abilities
{
    public interface IAbilityEvents
    {
        event Action<UseContext> AbilityStarted;
        event Action<UseContext> AbilityFinished;
    }
}