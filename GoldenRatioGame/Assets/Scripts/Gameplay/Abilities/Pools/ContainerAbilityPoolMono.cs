using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IM.Abilities
{
    public class ContainerAbilityPoolMono : MonoBehaviour, IContainerAbilityPool
    {
        private readonly IContainerAbilityPool _containerAbilityPool = new ContainerAbilityPool();
        
        public IEnumerator<IAbilityReadOnly> GetEnumerator() => _containerAbilityPool.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_containerAbilityPool).GetEnumerator();
        public int Count => _containerAbilityPool.Count;
        public bool Contains(IAbilityReadOnly item) => _containerAbilityPool.Contains(item);
        IReadOnlyCollection<IAbilityContainer> IContainerAbilityPoolReadOnly.AbilityContainers => ((IContainerAbilityPoolReadOnly)_containerAbilityPool).AbilityContainers;
        ICollection<IAbilityContainer> IContainerAbilityPool.AbilityContainers => _containerAbilityPool.AbilityContainers;
    }
}