using System.Collections.Generic;
using IM.Commands;
using IM.Graphs;

namespace IM.Abilities
{
    public class CommandAbilityPool : ICommandAbilityPool, INotifyOnEditingEnded
    {
        private readonly List<IAbilityReadOnly> _abilities = new();
        private readonly IAddAbilityCommandFactory _addFactory;
        private readonly IRemoveAbilityCommandFactory _removeFactory;
        private CommandStack _commands;

        public int CommandsToUndoCount => _commands.CommandsToUndoCount;
        public int CommandsToRedoCount => _commands.CommandsToRedoCount;
        public IReadOnlyCollection<IAbilityReadOnly> Abilities=> _abilities;

        public CommandAbilityPool() : this( new AddAbilityCommandFactory(), new RemoveAbilityCommandFactory())
        {
            
        }
        
        public CommandAbilityPool(IAddAbilityCommandFactory addFactory, IRemoveAbilityCommandFactory removeFactory)
        {
            _addFactory = addFactory;
            _removeFactory = removeFactory;
        }

        public void AddAbility(IAbilityReadOnly ability)
        {
            ICommand addCommand = _addFactory.Create(ability,_abilities);
            _commands.ExecuteAndPush(addCommand);
        }

        public void RemoveAbility(IAbilityReadOnly ability)
        {
            ICommand removeCommand = _removeFactory.Create(ability,_abilities);
            _commands.ExecuteAndPush(removeCommand);
        }
        
        public bool Contains(IAbilityReadOnly ability) => _abilities.Contains(ability);
        
        public bool CanUndo(int count) => _commands.CanUndo(count);
        public bool CanRedo(int count) => _commands.CanRedo(count);
        public void Undo(int count) => _commands.Undo(count);
        public void Redo(int count) => _commands.Redo(count);
        
        public void OnEditingEnded() 
        {
            ClearUndoCommands();
            ClearRedoCommands();
        }
        
        public void ClearUndoCommands() => _commands.ClearUndoCommands();
        public void ClearRedoCommands() => _commands.ClearRedoCommands();
    }
}