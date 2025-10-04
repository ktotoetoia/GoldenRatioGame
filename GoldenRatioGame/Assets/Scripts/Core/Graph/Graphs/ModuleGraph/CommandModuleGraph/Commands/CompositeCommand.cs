using System;
using System.Collections.Generic;
using IM.Commands;

namespace IM.Graphs
{
    public class CompositeCommand : ICommand
    {
        private readonly IReadOnlyList<ICommand> _commands;
        private bool _isExecuted;
        
        public CompositeCommand(IReadOnlyList<ICommand> commands)
        {
            _commands = commands ?? throw new ArgumentNullException(nameof(commands));
        }
        
        public void Execute()
        {
            if(_isExecuted)  throw new InvalidOperationException("Command already executed");
            
            foreach (var command in _commands)
            {
                command.Execute();
            }
            
            _isExecuted = true;
        }

        public void Undo()
        {
            if(!_isExecuted) throw new InvalidOperationException("Command must be executed before undo");

            for (int i = _commands.Count - 1; i >= 0; i--)
            {
                _commands[i].Undo();
            }
            
            _isExecuted = false;
        }
    }
}