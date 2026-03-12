using System.Collections.Generic;

namespace IM.Abilities
{
    public interface IAbilityPoolReadOnly
    {
        IReadOnlyCollection<IAbilityReadOnly> Abilities { get; }
        
        bool Contains(IAbilityReadOnly ability);
    }
}