using System;
using System.Collections.Generic;
using System.Linq;
using IM.Abilities;
using IM.StateMachines;
using UnityEngine;

namespace Tests
{
    public class AbilityUseState : State
    {
        private readonly IAbilityUser<IKeyAbilityPool> _abilityUser;
        private readonly Func<AbilityUseContext> _getUseContext;

        public AbilityUseState(IAbilityUser<IKeyAbilityPool> abilityUser, Func<AbilityUseContext> getUseContext)
        {
            _abilityUser = abilityUser;
            _getUseContext = getUseContext;
        }
        
        public override void Update()
        {
            IEnumerable<IAbilityReadOnly> abilities = _abilityUser.AbilityPool.KeyMap
                .Where(x => Input.GetKey(x.Key))
                .Select(x => x.Value);
            
            _abilityUser.ResolveRequestedAbilities(abilities,_getUseContext());
        }
    }
}