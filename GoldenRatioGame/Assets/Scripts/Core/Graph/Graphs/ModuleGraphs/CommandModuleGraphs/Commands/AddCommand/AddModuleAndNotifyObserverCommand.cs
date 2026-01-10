using System;
using IM.Commands;

namespace IM.Graphs
{
    public class AddModuleAndNotifyObserverCommand : Command
    {
        private readonly IModuleGraphObserver _observer;
        private readonly IModule _module;
        private readonly ICommand  _command;
        
        public AddModuleAndNotifyObserverCommand(IModule module, ICommand command, IModuleGraphObserver observer)
        {
            _observer = observer ?? throw new ArgumentNullException(nameof(observer));
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _command = command ?? throw new ArgumentNullException(nameof(command));
        }

        protected override void InternalExecute()
        {
            _command.Execute();
            _observer.OnModuleAdded(_module);
        }

        protected override void InternalUndo()
        {
            _command.Undo();
            _observer.OnModuleRemoved(_module);
        }
    }
}