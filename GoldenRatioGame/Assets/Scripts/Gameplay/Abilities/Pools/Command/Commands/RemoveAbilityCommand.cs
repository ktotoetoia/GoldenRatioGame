using System;
using System.Collections.Generic;
using IM.Commands;

namespace IM.Abilities
{
    public class RemoveAbilityCommand : Command
    {
        private readonly ICollection<IAbilityReadOnly> _abilities;
        private readonly IAbilityReadOnly _toAdd;
        
        public RemoveAbilityCommand(ICollection<IAbilityReadOnly>  abilities, IAbilityReadOnly toAdd)
        {
            _abilities = abilities ?? throw new ArgumentNullException(nameof(abilities));
            _toAdd = toAdd  ?? throw new ArgumentNullException(nameof(toAdd));
        }

        protected override void InternalExecute()
        {
            _abilities.Remove(_toAdd);
        }

        protected override void InternalUndo()
        {
            _abilities.Add(_toAdd);
        }
    }
}