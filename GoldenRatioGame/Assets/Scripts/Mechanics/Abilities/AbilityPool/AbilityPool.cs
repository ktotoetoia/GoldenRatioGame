using System.Collections.Generic;

namespace IM.Abilities
{
    public class AbilityPool : IAbilityPool
    {
        private readonly List<IAbility> _abilities = new();

        public IReadOnlyCollection<IAbility> Abilities => _abilities;
        
        public bool Contains(IAbility ability)
        {
            return _abilities.Contains(ability);
        }

        public void AddAbility(IAbility ability)
        {
            if (!_abilities.Contains(ability))
            {
                _abilities.Add(ability);
            }
        }

        public void RemoveAbility(IAbility ability)
        {
            _abilities.Remove(ability);
        }
    }
}