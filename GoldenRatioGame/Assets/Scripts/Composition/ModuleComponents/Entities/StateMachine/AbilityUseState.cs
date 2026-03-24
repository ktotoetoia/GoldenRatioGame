using System;
using System.Collections.Generic;
using IM.Abilities;
using IM.StateMachines;

namespace IM
{
    public class AbilityUseState : State
    {
        private readonly IAbilityUser<IAbilityPoolReadOnly> _abilityUser;
        private readonly Func<AbilityUseContext> _getUseContext;
        private readonly Func<IEnumerable<IAbilityReadOnly>, IEnumerable<IAbilityReadOnly>> _getRequestedAbilities;
        
        public AbilityUseState(IAbilityUser<IAbilityPoolReadOnly> abilityUser, Func<AbilityUseContext> getUseContext,Func<IEnumerable<IAbilityReadOnly>, IEnumerable<IAbilityReadOnly>> getRequestedAbilities)
        {
            _abilityUser = abilityUser;
            _getUseContext = getUseContext;
            _getRequestedAbilities = getRequestedAbilities;
        }

        public override void Update()
        {
            _abilityUser.ResolveRequestedAbilities(_getRequestedAbilities(_abilityUser.AbilityPool.Abilities),_getUseContext());
        }
    }
}