using System;

namespace IM.Commands
{
    public class Command : ICommand
    {
        public bool IsExecuted { get; private set; }

        public void Execute()
        {
            if (IsExecuted) throw new InvalidOperationException("Command already executed");       
            InternalExecute();
            IsExecuted = true;
        }

        public void Undo()
        {
            if (!IsExecuted) throw new InvalidOperationException("Command must be executed before undo");
            InternalUndo();
            IsExecuted = false;
        }

        protected virtual void InternalExecute()
        {

        }

        protected virtual void InternalUndo()
        {
            
        }
    }
}