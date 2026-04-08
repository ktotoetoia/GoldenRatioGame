using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace IM.Abilities
{
    public class CompositionAbilityPool : IAbilityPoolReadOnly
    {
        private readonly IEnumerable<IAbilityPoolReadOnly> _abilityPools;
        public IReadOnlyCollection<IAbilityReadOnly> Abilities => _abilityPools.SelectMany(x => x).ToList();

        public CompositionAbilityPool(IEnumerable<IAbilityPoolReadOnly> abilityPools)
        {
            _abilityPools = abilityPools;
        }
        
        public bool Contains(IAbilityReadOnly target)
        {
            return _abilityPools.Any(abilityPool => abilityPool.Any(ability => target == ability));
        }

        public IEnumerator<IAbilityReadOnly> GetEnumerator() => Abilities.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Abilities.GetEnumerator();

        public int Count { get; }
    }
}