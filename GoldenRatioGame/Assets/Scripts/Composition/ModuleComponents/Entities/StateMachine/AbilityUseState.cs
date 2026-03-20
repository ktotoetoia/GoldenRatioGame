using System;
using System.Collections.Generic;
using System.Linq;
using IM.Abilities;
using IM.StateMachines;
using UnityEngine;

namespace IM
{
    public class AbilityUseState : State
    {
        private readonly IAbilityUser<IAbilityPoolReadOnly> _abilityUser;
        private readonly Func<AbilityUseContext> _getUseContext;
        private readonly Func<IAbilityReadOnly,KeyCode> _getKeyForAbility;

        public AbilityUseState(IAbilityUser<IAbilityPoolReadOnly> abilityUser, Func<AbilityUseContext> getUseContext,Func<IAbilityReadOnly,KeyCode> getKeyForAbility)
        {
            _abilityUser = abilityUser;
            _getUseContext = getUseContext;
            _getKeyForAbility = getKeyForAbility;
        }

        public override void Update()
        {
            IEnumerable<IAbilityReadOnly> abilities = _abilityUser.AbilityPool.Abilities
                .Where(x => Input.GetKey(_getKeyForAbility(x)));
            
            _abilityUser.ResolveRequestedAbilities(abilities,_getUseContext());
        }
    }
}