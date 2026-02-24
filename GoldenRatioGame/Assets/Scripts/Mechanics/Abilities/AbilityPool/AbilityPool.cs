using System.Collections.Generic;

namespace IM.Abilities
{
    public class AbilityPool : IAbilityPool
    {
        private readonly List<IAbilityReadOnly> _abilities = new();

        public IReadOnlyCollection<IAbilityReadOnly> Abilities => _abilities;
        
        public bool Contains(IAbilityReadOnly ability)
        {
            return _abilities.Contains(ability);
        }

        public void AddAbility(IAbilityReadOnly ability)
        {
            if (!_abilities.Contains(ability))
            {
                _abilities.Add(ability);
            }
        }

        public void RemoveAbility(IAbilityReadOnly ability)
        {
            _abilities.Remove(ability);
        }
    }
}