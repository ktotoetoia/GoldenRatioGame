using System;
using System.Collections.Generic;
using IM.Commands;

namespace IM.Abilities
{
    public class AddAbilityCommand : Command
    {
        private readonly ICollection<IAbilityReadOnly> _abilities;
        private readonly IAbilityReadOnly _toAdd;
        
        public AddAbilityCommand(ICollection<IAbilityReadOnly>  abilities, IAbilityReadOnly toAdd)
        {
            _abilities = abilities ?? throw new ArgumentNullException(nameof(abilities));
            _toAdd = toAdd  ?? throw new ArgumentNullException(nameof(toAdd));
        }

        protected override void InternalExecute()
        {
            _abilities.Add(_toAdd);
        }

        protected override void InternalUndo()
        {
            _abilities.Remove(_toAdd);
        }
    }
}