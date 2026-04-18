using System;
using IM.Abilities;
using UnityEngine;

namespace IM.Modules
{
    public class AbilityPoolConverter : MonoBehaviour, IComponentConverter
    {
        public Type MutableType => typeof(ContainerAbilityPool);
        public Type ReadOnlyType => typeof(ContainerAbilityPool);

        public object CreateReadOnly() => new ContainerAbilityPool();
        public object ToReadOnly(object mutable) => new ContainerAbilityPool((mutable as IContainerAbilityPool).AbilityContainers);
        public object ToMutable(object readOnly) => new ContainerAbilityPool((readOnly as IContainerAbilityPool).AbilityContainers);
    }
}