using System.Collections.Generic;
using IM.Commands;

namespace IM.Graphs
{
    public class CommandModuleGraph : ICommandModuleGraph, INotifyOnEditingEnded
    {
        private readonly CommandStack _commands = new();
        private readonly List<IModule> _modules = new();
        private readonly List<IConnection> _connections = new();
        private readonly IAddModuleCommandFactory _addModuleCommandFactory;
        private readonly IRemoveModuleCommandFactory _removeModuleCommandFactory;
        private readonly IConnectCommandFactory _connectCommandFactory;
        private readonly IDisconnectCommandFactory _disconnectCommandFactory;

        public IEnumerable<IModule> Modules => _modules;
        public IEnumerable<INode> Nodes => _modules;
        public IEnumerable<IConnection> Connections => _connections;
        public IEnumerable<IEdge> Edges => _connections;
        public int CommandsToUndoCount => _commands.CommandsToUndoCount;
        public int CommandsToRedoCount => _commands.CommandsToRedoCount;

        public CommandModuleGraph() : this(new AddModuleCommandFactory(),new RemoveAndDisconnectCommandFactory(),new ConnectCommandFactory(), new DisconnectCommandFactory())
        {
            
        }

        public CommandModuleGraph(
            IAddModuleCommandFactory addModuleCommandFactory,
            IRemoveModuleCommandFactory removeModuleCommandFactory,
            IConnectCommandFactory connectCommandFactory,
            IDisconnectCommandFactory disconnectCommandFactory)
        {
            _addModuleCommandFactory = addModuleCommandFactory;
            _removeModuleCommandFactory = removeModuleCommandFactory;
            _connectCommandFactory = connectCommandFactory;
            _disconnectCommandFactory = disconnectCommandFactory;
        }

        public void AddModule(IModule module)
        {
            ICommand command = _addModuleCommandFactory.Create(module, _modules);
            _commands.ExecuteAndPush(command);
        }

        public void RemoveModule(IModule module)
        {
            ICommand command = _removeModuleCommandFactory.Create(module, _modules, _connections);
            _commands.ExecuteAndPush(command);
        }

        public IConnection Connect(IPort output, IPort input)
        {
            IConnectCommand command = _connectCommandFactory.Create(output, input, _connections);
            _commands.ExecuteAndPush(command);
            
            return command.Connection;
        }

        public void Disconnect(IConnection connection)
        {
            ICommand command = _disconnectCommandFactory.Create(connection, _connections);
            _commands.ExecuteAndPush(command);
        }
        
        public void AddAndConnect(IModule module, IPort ownerPort, IPort targetPort)
        {
            ICommand command = new CompositeCommand(new List<ICommand>
            {
                _addModuleCommandFactory.Create(module, _modules),
                _connectCommandFactory.Create(ownerPort, targetPort, _connections)
            });
            
            _commands.ExecuteAndPush(command);
        }

        public void OnEditingEnded()
        {
            ClearUndoCommands();
            ClearRedoCommands();
        }
        
        public void Undo(int count) => _commands.Undo(count);
        public void Redo(int count) => _commands.Redo(count);
        public bool CanUndo(int count) => _commands.CanUndo(count);
        public bool CanRedo(int count) => _commands.CanRedo(count);
        public void ClearUndoCommands() => _commands.ClearUndoCommands();
        public void ClearRedoCommands() => _commands.ClearRedoCommands();
        
        public bool Contains(IModule module)
        {
            return _modules.Contains(module);
        }

        public bool Contains(IConnection connection)
        {
            return _connections.Contains(connection);
        }
    }
}