using System;
using IM.Abilities;
using IM.StateMachines;

namespace Tests
{
    public class AbilityUseState : State
    {
        private readonly IAbilityUser<IAbilityPoolReadOnly> _abilityUser;

        public IAbilityReadOnly Ability { get; set; }
        public bool Finished => Ability is not IChannelAbilityReadOnly { IsChanneling: true };

        public AbilityUseState(IAbilityUser<IAbilityPoolReadOnly> abilityUser)
        {
            _abilityUser = abilityUser;
        }
        
        public override void OnEnter()
        {
            if(Ability == null) throw new InvalidOperationException("Ability must be set before transitioning to this state.");
            _abilityUser.UseAbility(Ability);
        }

        public override void OnExit()
        {
            Ability = null;
        }
    }
}