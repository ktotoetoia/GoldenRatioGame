using System.Collections.Generic;

namespace IM.Abilities
{
    public interface IAbilityPoolReadOnly : IReadOnlyCollection<IAbilityReadOnly>
    {
        bool Contains(IAbilityReadOnly item);
    }
}