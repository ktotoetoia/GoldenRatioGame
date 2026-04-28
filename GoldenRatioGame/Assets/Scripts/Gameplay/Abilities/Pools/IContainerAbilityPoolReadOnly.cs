using System.Collections.Generic;

namespace IM.Abilities
{
    public interface IContainerAbilityPoolReadOnly : IAbilityPoolReadOnly
    {
        IReadOnlyCollection<IAbilityContainer> AbilityContainers { get; }
    }
}