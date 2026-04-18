using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace IM.Abilities
{
    public class ContainerAbilityPool : IContainerAbilityPool
    {
        private readonly HashSet<IAbilityContainer> _abilityContainers;
        
        public int Count => _abilityContainers.Count;
        public ICollection<IAbilityContainer> AbilityContainers => _abilityContainers;

        public ContainerAbilityPool() :this(new HashSet<IAbilityContainer>())
        {
            
        }
        
        public ContainerAbilityPool(IEnumerable<IAbilityContainer> abilityContainers)
        {
            _abilityContainers = new HashSet<IAbilityContainer>(abilityContainers);
        }
        
        public IEnumerator<IAbilityReadOnly> GetEnumerator() => _abilityContainers.Select(x => x.Ability).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public bool Contains(IAbilityReadOnly item) => _abilityContainers.Any(x => x.Ability == item);
    }
}