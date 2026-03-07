using System;
using System.Collections.Generic;
using System.Linq;
using IM.Abilities;

namespace IM.UI
{
    public class AbilityPoolDraft : IAbilityPoolDraft
    {
        private IAbilityPool _abilityPool;
        private List<IAbilityReadOnly> _abilities;
        
        public IReadOnlyCollection<IAbilityReadOnly> Abilities => _abilities;
        public event Action<IAbilityReadOnly> AbilityAdded;
        public event Action<IAbilityReadOnly> AbilityRemoved;

        public AbilityPoolDraft(IAbilityPool abilityPool)
        {
            _abilityPool = abilityPool;
            _abilities = new List<IAbilityReadOnly>(abilityPool.Abilities);
        }
        
        public void AddAbility(IAbilityReadOnly ability)
        {
            _abilities.Add(ability);
            AbilityAdded?.Invoke(ability);
        }

        public void RemoveAbility(IAbilityReadOnly ability)
        {
            _abilities.Remove(ability);
            AbilityRemoved?.Invoke(ability);
        }
        
        public bool Contains(IAbilityReadOnly ability)
        {
            return _abilities.Contains(ability);
        }

        public void Commit()
        {
            foreach (var ability in _abilities.Where(x => !_abilityPool.Abilities.Contains(x)))
            {
                _abilityPool.AddAbility(ability);
            }
        }

        public void Rollback()
        {
            _abilityPool =  null;
            _abilities = null;
        }
    }
}