using System.Collections.Generic;
using IM.Values;

namespace IM.Abilities
{
    public interface IAbilityUser<out TAbilityPool> where TAbilityPool : IAbilityPoolReadOnly
    {
        public bool IsInterrupted { get; set; }
        TAbilityPool AbilityPool { get; }
        void ResolveRequestedAbilities(IEnumerable<KeyValuePair<IAbilityReadOnly, UseContext>> requestedAbilities);
    }
}