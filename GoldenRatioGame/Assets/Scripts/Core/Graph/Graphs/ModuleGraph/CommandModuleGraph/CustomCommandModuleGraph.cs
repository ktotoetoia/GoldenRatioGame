using System.Collections.Generic;
using IM.Commands;

namespace IM.Graphs
{
    public class CustomCommandModuleGraph : ICommandModuleGraph
    {
        private readonly CommandStack _commands = new();
        private readonly List<IModule> _modules = new();
        private readonly List<IConnection> _connections = new();
        private readonly IAddModuleCommandFactory _addModuleCommandFactory;
        private readonly IRemoveModuleCommandFactory _removeModuleCommandFactory;
        private readonly IConnectCommandFactory _connectCommandFactory;
        private readonly IDisconnectCommandFactory _disconnectCommandFactory;

        public IReadOnlyList<IModule> Modules => _modules;
        public IReadOnlyList<INode> Nodes => _modules;
        public IReadOnlyList<IConnection> Connections => _connections;
        public IReadOnlyList<IEdge> Edges => _connections;
        public int CommandsToUndoCount => _commands.CommandsToUndoCount;
        public int CommandsToRedoCount => _commands.CommandsToRedoCount;

        public CustomCommandModuleGraph() : this(new AddModuleCommandFactory(),new RemoveAndDisconnectCommandFactory(),new ConnectCommandFactory(), new DisconnectCommandFactory())
        {
            
        }

        public CustomCommandModuleGraph(
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

        public IConnection Connect(IModulePort output, IModulePort input)
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

        public void Undo(int count) => _commands.Undo(count);
        public void Redo(int count) => _commands.Redo(count);
        public bool CanUndo(int count) => _commands.CanUndo(count);
        public bool CanRedo(int count) => _commands.CanRedo(count);
        public void ClearUndoCommands() => _commands.ClearUndoCommands();
        public void ClearRedoCommands() => _commands.ClearRedoCommands();
    }
}