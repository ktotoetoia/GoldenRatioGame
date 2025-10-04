using System.Collections.Generic;
using System.Linq;

namespace IM.Graphs
{
    public class CommandModuleGraph : IModuleGraph
    {
        private readonly Stack<ICommand> _commands = new();
        private readonly Stack<ICommand> _undone = new();
        private readonly List<IModule> _modules = new();
        private readonly List<IConnection> _connections = new();

        public IReadOnlyList<INode> Nodes => _modules;
        public IReadOnlyList<IModule> Modules => _modules;
        public IReadOnlyList<IEdge> Edges => _connections;
        public IReadOnlyList<IConnection> Connections => _connections;
        
        public void AddModule(IModule module)
        {
            ICommand command = new AddModuleCommand(module,_modules);
            
            ExecuteAndPush(command);
        }

        public void RemoveModule(IModule module)
        {
            ICommand command = new RemoveAndDisconnectModule(module,_modules,_connections);
            
            ExecuteAndPush(command);
        }

        public IConnection Connect(IModulePort output, IModulePort input)
        {
            ConnectModulesCommand command = new ConnectModulesCommand(output, input,_connections);
            
            ExecuteAndPush(command);
            
            return command.Connection;
        }

        public void Disconnect(IConnection connection)
        {
            DisconnectModulesCommand command = new DisconnectModulesCommand(connection, _connections);
            
            ExecuteAndPush(command);
        }

        private void ExecuteAndPush(ICommand command)
        {
            command.Execute();
            _commands.Push(command);
            _undone.Clear();
        }

        public void UndoLast()
        {
            if(_commands.Count == 0) return;
            
            ICommand command = _commands.Pop();
            
            _undone.Push(command);
            command.Undo();
        }

        public void RedoLast()
        {
            if(_undone.Count == 0) return;
            
            ICommand command = _undone.Pop();
            
            _commands.Push(command);
            command.Execute();
        }
    }
}