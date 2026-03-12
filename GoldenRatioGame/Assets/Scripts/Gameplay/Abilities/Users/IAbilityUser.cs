using System.Collections.Generic;

namespace IM.Abilities
{
    public interface IAbilityUser<out TAbilityPool> where TAbilityPool : IAbilityPoolReadOnly
    {
        TAbilityPool AbilityPool { get; }
        
        void ResolveRequestedAbilities(IEnumerable<IAbilityReadOnly> requestedAbilities,
            AbilityUseContext abilityUseContext);
    }
}