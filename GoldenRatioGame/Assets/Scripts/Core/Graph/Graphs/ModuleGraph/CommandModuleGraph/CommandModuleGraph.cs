using System.Collections.Generic;
using IM.Commands;

namespace IM.Graphs
{
    public class CommandModuleGraph : ICommandModuleGraph
    {
        private readonly CommandStack _commands = new();
        private readonly List<IModule> _modules = new();
        private readonly List<IConnection> _connections = new();

        public IReadOnlyList<IModule> Modules => _modules;
        public IReadOnlyList<INode> Nodes => _modules;
        public IReadOnlyList<IConnection> Connections => _connections;
        public IReadOnlyList<IEdge> Edges => _connections;
        
        public int CommandsToUndoCount => _commands.CommandsToUndoCount;
        public int CommandsToRedoCount => _commands.CommandsToRedoCount;
        
        public void AddModule(IModule module)
        {
            ICommand command = new AddModuleCommand(module,_modules);
            
            _commands.ExecuteAndPush(command);
        }

        public void RemoveModule(IModule module)
        {
            ICommand command = new RemoveAndDisconnectModule(module,_modules,_connections);
            
            _commands.ExecuteAndPush(command);
        }

        public IConnection Connect(IModulePort output, IModulePort input)
        {
            ConnectModulesCommand command = new ConnectModulesCommand(output, input,_connections);
            
            _commands.ExecuteAndPush(command);
            
            return command.Connection;
        }

        public void Disconnect(IConnection connection)
        {
            DisconnectModulesCommand command = new DisconnectModulesCommand(connection, _connections);
            
            _commands.ExecuteAndPush(command);
        }
        
        public void Undo(int count)
        {
            _commands.Undo(count);
        }

        public void Redo(int count)
        {
            _commands.Redo(count);
        }

        public bool CanUndo(int count)
        {
            return _commands.CanUndo(count);
        }

        public bool CanRedo(int count)
        {
            return _commands.CanRedo(count);
        }

        public void ClearUndoCommands()
        {
            _commands.ClearUndoCommands();
        }

        public void ClearRedoCommands()
        {
            _commands.ClearRedoCommands();
        }
    }
}