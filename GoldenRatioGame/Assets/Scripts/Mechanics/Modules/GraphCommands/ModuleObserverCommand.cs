using System;
using System.Collections.Generic;
using System.Linq;
using IM.Commands;

namespace IM.Modules
{
    public class ModuleObserverCommand : Command
    {
        private readonly ICommand _command;
        private readonly IReadOnlyList<ICommandObserver> _observers;

        public bool Inverted { get; set; }

        public ModuleObserverCommand(
            ICommand command,
            IEnumerable<ICommandObserver> observers)
        {
            _command = command ?? throw new ArgumentNullException(nameof(command));

            _observers = new List<ICommandObserver>(
                observers ?? Enumerable.Empty<ICommandObserver>());
        }

        protected override void InternalExecute()
        {
            _command.Execute();

            for (int i = 0; i < _observers.Count; i++)
            {
                var observer = _observers[i];

                if (Inverted)
                    observer.OnModuleRemoved();
                else
                    observer.OnModuleAdded();
            }
        }

        protected override void InternalUndo()
        {
            _command.Undo();

            for (int i = _observers.Count - 1; i >= 0; i--)
            {
                var observer = _observers[i];

                if (Inverted)
                    observer.OnModuleAdded();
                else
                    observer.OnModuleRemoved();
            }
        }
    }
}