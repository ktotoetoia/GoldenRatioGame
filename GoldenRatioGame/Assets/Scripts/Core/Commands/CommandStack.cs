using System;
using System.Collections.Generic;

namespace IM.Commands
{
    public class CommandStack : ICommandUser
    {
        private readonly Stack<ICommand> _done = new();
        private readonly Stack<ICommand> _undone = new();
        
        public IEnumerable<ICommand> Done => _done;
        public IEnumerable<ICommand> Undone => _undone;

        public bool CanUndo => _done.Count > 0;
        public bool CanRedo => _undone.Count > 0;
        
        public void ExecuteAndPush(ICommand command)
        {
            try
            {
                command.Execute();
            }
            catch
            {
                return;
            }
            
            _done.Push(command);
            _undone.Clear();
        }

        public void Undo(int count)
        {
            for (int i = 0; i < count && _done.Count > 0; i++)
                UndoLast();
        }

        public void Redo(int count)
        {
            for (int i = 0; i < count && _undone.Count > 0; i++)
                RedoLast();
        }

        public void UndoLast()
        {
            if(!CanUndo) return;
            
            ICommand command = _done.Pop();
            
            _undone.Push(command);
            command.Undo();
        }

        public void RedoLast()
        {
            if(!CanRedo) return;
            
            ICommand command = _undone.Pop();
            
            _done.Push(command);
            command.Execute();
        }
    }
}