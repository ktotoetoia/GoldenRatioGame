using System;
using System.Collections.Generic;
using System.Linq;
using IM.Commands;

namespace IM.Graphs
{
    public class RemoveAndDisconnectModuleCommand : Command
    {
        private readonly ICollection<IModule> _modules;
        private readonly ICollection<IConnection> _connections;
        private readonly IModule _module;
        private ICommand _compositeCommand;
        
        public RemoveAndDisconnectModuleCommand(IModule module, ICollection<IModule> modules, ICollection<IConnection> connections)
        {
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _modules = modules ?? throw new ArgumentNullException(nameof(modules));
            _connections = connections ?? throw new ArgumentNullException(nameof(connections));
        }

        protected override void InternalExecute()
        {
            List<ICommand> commands = new List<ICommand>();
            
            foreach (IPort port in _module.Ports.Where(x => x.IsConnected))
            {
                commands.Add(new DisconnectModulesCommand(port.Connection,_connections));
            }
            
            commands.Add(new RemoveModuleCommand(_module,_modules));

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