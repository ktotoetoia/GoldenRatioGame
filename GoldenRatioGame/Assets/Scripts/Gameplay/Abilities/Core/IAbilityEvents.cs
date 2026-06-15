using System;
using IM.Values;

namespace IM.Abilities
{
    public interface IAbilityEvents
    {
        event Action<UseContext> AbilityWindUp;
        event Action<UseContext> AbilityFired;
    }
}