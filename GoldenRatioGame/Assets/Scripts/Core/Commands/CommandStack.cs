using System;
using System.Collections.Generic;

namespace IM.Commands
{
    public class CommandStack : ICommandUser
    {
        private readonly Stack<ICommand> _done = new();
        private readonly Stack<ICommand> _undone = new();

        public int CommandsToUndoCount => _done.Count;
        public int CommandsToRedoCount => _undone.Count;
        public IEnumerable<ICommand> Done => _done;
        public IEnumerable<ICommand> Undone => _undone;

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
            if (!CanUndo(count))
                throw new InvalidOperationException("Not enough commands to undo.");

            for (int i = 0; i < count; i++)
            {
                ICommand command = _done.Pop();
                command.Undo();
                _undone.Push(command);
            }
        }

        public void Redo(int count)
        {
            if (!CanRedo(count))
                throw new InvalidOperationException("Not enough commands to redo.");

            for (int i = 0; i < count; i++)
            {
                ICommand command = _undone.Pop();
                command.Execute();
                _done.Push(command);
            }
        }

        public void UndoLast() => Undo(1);
        public void RedoLast() => Redo(1);
        public bool CanUndo(int count) => _done.Count >= count;
        public bool CanRedo(int count) => _undone.Count >= count;
        public void ClearUndoCommands() => _done.Clear();
        public void ClearRedoCommands() => _undone.Clear();
    }
}