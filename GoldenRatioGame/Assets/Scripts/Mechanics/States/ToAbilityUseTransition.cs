using System;
using IM.Abilities;
using IM.StateMachines;

namespace Tests
{
    public class ToAbilityUseTransition : ITransition
    {
        private readonly AbilityUseState _to;
        private readonly Func<IAbilityReadOnly> _getAbility;
        private IAbilityReadOnly _ability;
        
        public IState From { get; }
        public IState To => _to;
        
        public ToAbilityUseTransition(IState from, AbilityUseState to, Func<IAbilityReadOnly> getAbility)
        {
            From = from;
            _to = to;
            _getAbility = getAbility;
        }
        
        public bool CanTransition()
        {
            _ability = _getAbility();

            if (_ability == null) return false;
            
            return true;
        }

        public void BeforeTransition()
        {
            _to.Ability = _ability;
        }
    }
}