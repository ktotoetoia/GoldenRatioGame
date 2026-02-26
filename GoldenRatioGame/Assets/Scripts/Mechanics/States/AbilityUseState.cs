using System;
using System.Collections.Generic;
using IM.Abilities;
using IM.StateMachines;
using UnityEngine;

namespace Tests
{
    public class AbilityUseState : State
    {
        private readonly IAbilityUser<IKeyAbilityPool> _abilityUser;
        
        public IAbilityReadOnly Ability { get; set; }

        public AbilityUseState(IAbilityUser<IKeyAbilityPool> abilityUser)
        {
            _abilityUser = abilityUser;
        }
        
        public override void OnEnter()
        {
            if(Ability == null) throw new InvalidOperationException("Ability must be set before transitioning to this state.");
            _abilityUser.TryUseAbility(Ability);
        }

        public override void Update()
        {
            foreach (KeyValuePair<KeyCode, IAbilityReadOnly> valuePair in  _abilityUser.AbilityPool.KeyMap)
            {
                if(Input.GetKeyDown(valuePair.Key)) _abilityUser.TryUseAbility(valuePair.Value);
            }   
        }

        public override void OnExit()
        {
            Ability = null;
        }
    }
}