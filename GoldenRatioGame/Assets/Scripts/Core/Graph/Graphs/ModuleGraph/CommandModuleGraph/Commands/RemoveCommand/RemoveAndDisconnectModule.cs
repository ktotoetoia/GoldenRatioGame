using System;
using System.Collections.Generic;
using System.Linq;
using IM.Commands;

namespace IM.Graphs
{
    public class RemoveAndDisconnectModule : ICommand
    {
        private readonly ICollection<IModule> _modules;
        private readonly ICollection<IConnection> _connections;
        private readonly IModule _module;
        private ICommand _compositeCommand;
        private bool _isExecuted;
        
        public RemoveAndDisconnectModule(IModule module, ICollection<IModule> modules, ICollection<IConnection> connections)
        {
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _modules = modules ?? throw new ArgumentNullException(nameof(modules));
            _connections = connections ?? throw new ArgumentNullException(nameof(connections));
        }
        
        public void Execute()
        {
            if(_isExecuted)  throw new InvalidOperationException("Command already executed");
            
            List<ICommand> commands = new List<ICommand>();
            
            foreach (IPort port in _module.Ports.Where(x => x.IsConnected))
            {
                commands.Add(new DisconnectModulesCommand(port.Connection,_connections));
            }
            
            commands.Add(new RawRemoveModuleCommand(_module,_modules));

            _compositeCommand = new CompositeCommand(commands);
            _compositeCommand.Execute();
            _isExecuted = true;
        }

        public void Undo()
        {
            if(!_isExecuted) throw new InvalidOperationException("Command must be executed before undo");
            
            _compositeCommand.Undo();
            _compositeCommand = null;
            _isExecuted = false;
        }
    }
}