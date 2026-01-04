using System;
using System.Collections.Generic;
using IM.Commands;

namespace IM.Graphs
{
    public class CompositeCommand : Command
    {
        private readonly IReadOnlyList<ICommand> _commands;
        
        public CompositeCommand(IReadOnlyList<ICommand> commands)
        {
            _commands = commands ?? throw new ArgumentNullException(nameof(commands));
        }

        protected override void InternalExecute()
        {
            foreach (var command in _commands)
            {
                command.Execute();
            }
        }

        protected override void InternalUndo()
        {
            for (int i = _commands.Count - 1; i >= 0; i--)
            {
                _commands[i].Undo();
            }
        }
    }
}