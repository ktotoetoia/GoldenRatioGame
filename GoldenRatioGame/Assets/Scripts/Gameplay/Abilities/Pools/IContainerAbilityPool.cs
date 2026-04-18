using System.Collections.Generic;

namespace IM.Abilities
{
    public interface IContainerAbilityPool : IAbilityPoolReadOnly
    {
        ICollection<IAbilityContainer> AbilityContainers { get; }
    }
}