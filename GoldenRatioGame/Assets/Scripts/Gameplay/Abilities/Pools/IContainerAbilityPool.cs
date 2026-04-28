using System.Collections.Generic;

namespace IM.Abilities
{
    public interface IContainerAbilityPool : IContainerAbilityPoolReadOnly
    {
        new ICollection<IAbilityContainer> AbilityContainers { get; }
    }
}