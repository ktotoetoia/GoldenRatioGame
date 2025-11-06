using System.Collections.Generic;
using IM.Base;
using IM.Commands;

namespace IM.Graphs
{
    public class CommandModuleGraph : ICommandModuleGraph
    {
        private readonly CommandStack _commands = new();
        private readonly List<IModule> _modules = new();
        private readonly List<IConnection> _connections = new();
        private readonly IFactory<ICommand, IModule, ICollection<IModule>> _addModuleCommandFactory;
        private readonly IFactory<ICommand, IModule, ICollection<IModule>, ICollection<IConnection>> _removeModuleCommandFactory;
        private readonly IFactory<IConnectCommand, IPort, IPort, ICollection<IConnection>> _connectCommandFactory;
        private readonly IFactory<ICommand, IConnection, ICollection<IConnection>> _disconnectCommandFactory;

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
            IFactory<ICommand, IModule, ICollection<IModule>> addModuleCommandFactory,
            IFactory<ICommand, IModule, ICollection<IModule>, ICollection<IConnection>> removeModuleCommandFactory,
            IFactory<IConnectCommand, IPort, IPort, ICollection<IConnection>> connectCommandFactory,
            IFactory<ICommand, IConnection, ICollection<IConnection>> disconnectCommandFactory)
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

        public void Undo(int count) => _commands.Undo(count);
        public void Redo(int count) => _commands.Redo(count);

        public bool CanUndo(int count) => _commands.CanUndo(count);
        public bool CanRedo(int count) => _commands.CanRedo(count);
        public void ClearUndoCommands() => _commands.ClearUndoCommands();
        public void ClearRedoCommands() => _commands.ClearRedoCommands();
    }
}