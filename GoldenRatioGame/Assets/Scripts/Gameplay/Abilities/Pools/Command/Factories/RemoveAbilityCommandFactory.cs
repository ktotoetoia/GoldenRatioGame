using System.Collections.Generic;
using IM.Commands;

namespace IM.Abilities
{
    public class RemoveAbilityCommandFactory : IRemoveAbilityCommandFactory
    {
        public ICommand Create(IAbilityReadOnly param1, ICollection<IAbilityReadOnly> param2)
        {
            return new RemoveAbilityCommand(param2,param1);
        }
    }
}