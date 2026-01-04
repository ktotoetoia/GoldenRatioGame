using System;
using System.Collections.Generic;
using System.Linq;
using IM.Commands;

namespace IM.Graphs
{
    public class RemoveAndDisconnectModuleNotifyObserverCommand : Command
    {        
        private readonly ICollection<IModule> _modules;
        private readonly ICollection<IConnection> _connections;
        private readonly IModule _module;
        private readonly IModuleGraphObserver _observer;
        private ICommand _compositeCommand;
        
        public RemoveAndDisconnectModuleNotifyObserverCommand(IModule module, ICollection<IModule> modules, ICollection<IConnection> connections, IModuleGraphObserver observer)
        {
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _modules = modules ?? throw new ArgumentNullException(nameof(modules));
            _connections = connections ?? throw new ArgumentNullException(nameof(connections));
            _observer = observer  ?? throw new ArgumentNullException(nameof(observer));;
        }

        protected override void InternalExecute()
        {
            List<ICommand> commands = new List<ICommand>();
            
            foreach (IPort port in _module.Ports.Where(x => x.IsConnected))
            {
                commands.Add(new DisconnectModuleAndNotifyObserverCommand(port.Connection, new DisconnectModulesCommand(port.Connection, _connections),_observer));
            }
            
            commands.Add(new RemoveAndNotifyObserverCommand(_module,new RemoveModuleCommand(_module,_modules),_observer));

            _compositeCommand = new CompositeCommand(commands);
            _compositeCommand.Execute();
        }

        protected override void InternalUndo()
        {
            _compositeCommand.Undo();
            _compositeCommand = null;
        }
    }
}