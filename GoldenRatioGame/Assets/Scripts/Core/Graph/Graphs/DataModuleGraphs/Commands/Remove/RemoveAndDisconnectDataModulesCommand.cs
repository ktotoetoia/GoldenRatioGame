using System;
using System.Collections.Generic;
using System.Linq;
using IM.Commands;

namespace IM.Graphs
{
    public class RemoveAndDisconnectDataModulesCommand<T> : Command
    {
        private readonly ICollection<IDataModule<T>> _modules;
        private readonly ICollection<IDataConnection<T>> _connections;
        private readonly IDataModule<T> _module;
        private ICommand _compositeCommand;
        
        public RemoveAndDisconnectDataModulesCommand(IDataModule<T> module, ICollection<IDataModule<T>> modules, ICollection<IDataConnection<T>> connections)
        {
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _modules = modules ?? throw new ArgumentNullException(nameof(modules));
            _connections = connections ?? throw new ArgumentNullException(nameof(connections));
        }

        protected override void InternalExecute()
        {
            List<ICommand> commands = new List<ICommand>();
            
            foreach (IDataPort<T> port in _module.DataPorts.Where(x => x.IsConnected))
            {
                commands.Add(new DisconnectDataModulesCommand<T>(port.DataConnection,_connections));
            }

            commands.Add(new RemoveDataModuleCommand<T>(_module,_modules));
            
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