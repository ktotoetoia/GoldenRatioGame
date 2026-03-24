using System.Collections.Generic;
using IM.Commands;
using IM.LifeCycle;

namespace IM.Abilities
{
    public interface IRemoveAbilityCommandFactory : IFactory<ICommand,IAbilityReadOnly,ICollection<IAbilityReadOnly>>
    {
        
    }
}