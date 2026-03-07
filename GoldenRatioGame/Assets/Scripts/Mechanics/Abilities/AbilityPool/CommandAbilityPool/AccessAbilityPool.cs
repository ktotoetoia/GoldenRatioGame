using System;
using System.Collections.Generic;

namespace IM.Abilities
{
    public class AccessAbilityPool : IAccessAbilityPool
    {
        private readonly IAbilityPool _abilityPool;
        public IReadOnlyCollection<IAbilityReadOnly> Abilities => !TryUse() ? null : _abilityPool.Abilities;

        public bool CanUse { get; set; }
        public bool ThrowIfCantUse { get; set; }

        public AccessAbilityPool(IAbilityPool abilityPool)
        {
            _abilityPool = abilityPool;
        }
        
        public bool Contains(IAbilityReadOnly ability)
        {
            if(!TryUse()) return false;
            return _abilityPool.Contains(ability);
        }

        public void AddAbility(IAbilityReadOnly ability)
        {
            if(!TryUse()) return ;
            _abilityPool.AddAbility(ability);
        }

        public void RemoveAbility(IAbilityReadOnly ability)
        {
            if(!TryUse()) return ;
            _abilityPool.RemoveAbility(ability);
        }

        private bool TryUse()
        {
            if (CanUse) return true;

            if (ThrowIfCantUse)
                throw new InvalidOperationException("Ability Pool access denied (CanUse = false).");

            return false;
        }
    }
}