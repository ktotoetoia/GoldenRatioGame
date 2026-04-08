using System.Collections.Generic;

namespace IM.Abilities
{
    public interface IAbilityPool : ICollection<IAbilityReadOnly>, IAbilityPoolReadOnly
    {
        
    }
}