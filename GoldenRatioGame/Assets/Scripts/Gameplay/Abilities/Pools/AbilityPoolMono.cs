using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IM.Abilities
{
    public class AbilityPoolMono : MonoBehaviour, IAbilityPool, IAbilityPoolEvents, IAbilityPoolDraftContainer
    {
        private readonly IAbilityPool _abilityPool = new  AbilityPool();
        private readonly IAbilityPool _draft = new AbilityPool();
        public event Action<IAbilityReadOnly> AbilityAdded;
        public event Action<IAbilityReadOnly> AbilityRemoved;
        
        public IReadOnlyCollection<IAbilityReadOnly> Abilities => _abilityPool.Abilities;
        public IAbilityPoolReadOnly Draft=> _draft;
        
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

        public IAbilityPool EditDraft() => _draft;
        
        public void Commit()
        {
            var toRemove = _abilityPool.Abilities
                .Where(a => !Draft.Contains(a))
                .ToList();

            foreach (var ability in toRemove)
                RemoveAbility(ability);

            var toAdd = Draft.Abilities
                .Where(a => !_abilityPool.Contains(a))
                .ToList();

            foreach (var ability in toAdd)
                AddAbility(ability);
        }
    }
}