using System.Collections.Generic;
using IM.Commands;
using IM.LifeCycle;

namespace IM.Abilities
{
    public interface IAddAbilityCommandFactory : IFactory<ICommand,IAbilityReadOnly,ICollection<IAbilityReadOnly>>
    {
        
    }
}