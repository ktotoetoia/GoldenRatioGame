using System;
using System.Linq;
using IM.Abilities;
using UnityEngine;

namespace IM.Modules
{
    public class AbilityPoolConverter : MonoBehaviour, IComponentConverter
    {
        private readonly AbilityContainerMapper _abilityContainerWrapper = new();
        public Type MutableType => typeof(ContainerAbilityPool);
        public Type ReadOnlyType => typeof(ContainerAbilityPool);

        public object CreateReadOnly() => new ContainerAbilityPool();
        public object ToReadOnly(object mutable)
        {
            return new ContainerAbilityPool(((ContainerAbilityPool)mutable).AbilityContainers.Select(x => _abilityContainerWrapper.UnWrap(x)));
        }
        public object ToMutable(object readOnly)
        {
            return new ContainerAbilityPool(((ContainerAbilityPool)readOnly).AbilityContainers.Select(x => _abilityContainerWrapper.Wrap(x)));
        }
    }
}