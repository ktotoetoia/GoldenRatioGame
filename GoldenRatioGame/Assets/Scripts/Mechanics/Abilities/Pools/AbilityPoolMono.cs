using System;
using System.Collections.Generic;
using UnityEngine;

namespace IM.Abilities
{
    public class AbilityPoolMono : MonoBehaviour, IAbilityPool, IAbilityPoolEvents
    {
        private readonly IAbilityPool _abilityPool = new  AbilityPool();
        public IReadOnlyCollection<IAbilityReadOnly> Abilities => _abilityPool.Abilities;
        public event Action<IAbilityReadOnly> AbilityAdded;
        public event Action<IAbilityReadOnly> AbilityRemoved;
        
        public bool Contains(IAbilityReadOnly ability)
        {
            return _abilityPool.Contains(ability);
        }

        public void AddAbility(IAbilityReadOnly ability)
        {
            _abilityPool.AddAbility(ability);
            AbilityAdded?.Invoke(ability);
        }

        public void RemoveAbility(IAbilityReadOnly ability)
        {
            _abilityPool.RemoveAbility(ability);
            AbilityRemoved?.Invoke(ability);
        }
    }
}