using System.Collections.Generic;
using IM.Commands;

namespace IM.Graphs
{
    public class CommandDataModuleGraph<T> : ICommandDataModuleGraph<T>, INotifyOnEditingEnded
    {
        private readonly CommandStack _commands = new();
        private readonly List<IDataModule<T>> _modules = new();
        private readonly List<IDataConnection<T>> _connections = new();
        private readonly IAddDataModuleCommandFactory<T> _addFactory;
        private readonly IRemoveDataModuleCommandFactory<T> _removeFactory;
        private readonly IConnectDataModulesCommandFactory<T> _connectFactory;
        private readonly IDisconnectDataCommandFactory<T> _disconnectFactory;

        public IEnumerable<INode> Nodes => _modules;
        public IReadOnlyList<IModule> Modules => _modules;
        public IEnumerable<IDataModule<T>> DataModules => _modules;
        public IEnumerable<IEdge> Edges => _connections;
        public IReadOnlyList<IConnection> Connections => _connections;
        public IEnumerable<IDataConnection<T>> DataConnections => _connections;
        public int CommandsToUndoCount => _commands.CommandsToUndoCount;
        public int CommandsToRedoCount => _commands.CommandsToRedoCount;

        public CommandDataModuleGraph() : 
            this(new AddDataModuleCommandFactory<T>(),
                new RemoveAndDisconnectDataModulesCommandFactory<T>(),
                new ConnectDataModulesCommandFactory<T>(),
                new DisconnectDataModulesCommandFactory<T>())
        {
            
        }

        public CommandDataModuleGraph(
            IAddDataModuleCommandFactory<T> addFactory,
            IRemoveDataModuleCommandFactory<T> removeFactory,
            IConnectDataModulesCommandFactory<T> connectFactory,
            IDisconnectDataCommandFactory<T> disconnectFactory)
        {
            _addFactory = addFactory;
            _removeFactory = removeFactory;
            _connectFactory = connectFactory;
            _disconnectFactory = disconnectFactory;
        }

        public void Add(IDataModule<T> toAdd)
        {
            ICommand command = _addFactory.Create(toAdd, _modules);
            _commands.ExecuteAndPush(command);
        }

        public void Remove(IDataModule<T> module)
        {
            ICommand command = _removeFactory.Create(module, _modules, _connections);
            _commands.ExecuteAndPush(command);
        }

        public IDataConnection<T> Connect(IDataPort<T> port1, IDataPort<T> port2)
        {
            IDataConnectCommand<T> command = _connectFactory.Create(port1, port2, _connections);
            _commands.ExecuteAndPush(command);
            return command.Connection;
        }
        
        public void Disconnect(IDataConnection<T> connection)
        {
            ICommand command = _disconnectFactory.Create(connection, _connections);
            _commands.ExecuteAndPush(command);
        }

        public void AddAndConnect(IDataModule<T> module, IDataPort<T> ownerPort, IDataPort<T> targetPort)
        {
            ICommand command = new CompositeCommand(new List<ICommand>
            {
                _addFactory.Create(module, _modules),
                _connectFactory.Create(ownerPort, targetPort, _connections)
            });
            
            _commands.ExecuteAndPush(command);
        }

        public bool Contains(IModule module) => _modules.Contains(module as IDataModule<T>);
        public bool Contains(IConnection connection) => _connections.Contains(connection as IDataConnection<T>);
        public bool CanUndo(int count) => _commands.CanUndo(count);
        public bool CanRedo(int count) => _commands.CanRedo(count);
        public void Undo(int count) => _commands.Undo(count);
        public void Redo(int count) => _commands.Redo(count);
        public void ClearUndoCommands() => _commands.ClearUndoCommands();
        public void ClearRedoCommands() => _commands.ClearRedoCommands();
        public void OnEditingEnded()
        {
            ClearUndoCommands();
            ClearRedoCommands();
        }
    }
}