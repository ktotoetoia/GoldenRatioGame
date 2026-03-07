using System.Collections.Generic;
using IM.Commands;

namespace IM.Abilities
{
    public class AddAbilityCommandFactory : IAddAbilityCommandFactory
    {
        public ICommand Create(IAbilityReadOnly ability, ICollection<IAbilityReadOnly> abilities)
        {
            return new AddAbilityCommand(abilities, ability);
        }
    }
}