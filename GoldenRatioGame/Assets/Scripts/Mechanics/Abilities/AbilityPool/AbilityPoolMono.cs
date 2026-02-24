using System.Collections.Generic;
using UnityEngine;

namespace IM.Abilities
{
    public class AbilityPoolMono : MonoBehaviour, IAbilityPool
    {
        private readonly IAbilityPool _abilityPool = new  AbilityPool();
        public IReadOnlyCollection<IAbilityReadOnly> Abilities => _abilityPool.Abilities;
        
        public bool Contains(IAbilityReadOnly ability)
        {
            return _abilityPool.Contains(ability);
        }

        public void AddAbility(IAbilityReadOnly ability)
        {
            _abilityPool.AddAbility(ability);
        }

        public void RemoveAbility(IAbilityReadOnly ability)
        {
            _abilityPool.RemoveAbility(ability);
        }
    }
}