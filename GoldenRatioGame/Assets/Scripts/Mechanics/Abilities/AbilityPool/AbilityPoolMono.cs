using System.Collections.Generic;
using UnityEngine;

namespace IM.Abilities
{
    public class AbilityPoolMono : MonoBehaviour, IAbilityPool
    {
        private readonly IAbilityPool _abilityPool = new  AbilityPool();
        public IReadOnlyCollection<IAbility> Abilities => _abilityPool.Abilities;

        public void AddAbility(IAbility ability)
        {
            _abilityPool.AddAbility(ability);
        }

        public void RemoveAbility(IAbility ability)
        {
            _abilityPool.RemoveAbility(ability);
        }
    }
}