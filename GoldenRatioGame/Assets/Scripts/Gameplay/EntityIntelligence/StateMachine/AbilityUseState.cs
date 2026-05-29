using System;
using System.Collections.Generic;
using IM.Abilities;
using IM.StateMachines;
using IM.Values;

namespace IM
{
    public class AbilityUseState : State
    {
        private readonly IAbilityUser<IAbilityPoolReadOnly> _abilityUser;
        private readonly Func<IEnumerable<IAbilityReadOnly>, IEnumerable<KeyValuePair<IAbilityReadOnly, UseContext>>> _getRequestedAbilities;
        
        public AbilityUseState(IAbilityUser<IAbilityPoolReadOnly> abilityUser, Func<IEnumerable<IAbilityReadOnly>, IEnumerable<KeyValuePair<IAbilityReadOnly,UseContext>>> getRequestedAbilities)
        {
            _abilityUser = abilityUser;
            _getRequestedAbilities = getRequestedAbilities;
        }

        public override void Update()
        {
            _abilityUser.ResolveRequestedAbilities(_getRequestedAbilities(_abilityUser.AbilityPool));
        }
    }
}