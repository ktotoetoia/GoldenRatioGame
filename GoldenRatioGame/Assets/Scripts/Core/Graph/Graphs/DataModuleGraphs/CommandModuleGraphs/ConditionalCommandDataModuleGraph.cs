using System.Collections.Generic;

namespace IM.Graphs
{
    public class ConditionalCommandDataModuleGraph<T> : IConditionalCommandDataModuleGraph<T>
    {
        private readonly ICommandDataModuleGraph<T> _commandDataModuleGraph;
        private readonly IDataModuleGraphConditions<T> _conditions;

        public IEnumerable<IDataModule<T>> DataModules => _commandDataModuleGraph.DataModules;
        public IEnumerable<IDataConnection<T>> DataConnections => _commandDataModuleGraph.DataConnections;
        public IEnumerable<INode> Nodes => _commandDataModuleGraph.Nodes;
        public IEnumerable<IEdge> Edges => _commandDataModuleGraph.Edges;
        public IReadOnlyList<IModule> Modules => _commandDataModuleGraph.Modules;
        public IReadOnlyList<IConnection> Connections => _commandDataModuleGraph.Connections;

        public ConditionalCommandDataModuleGraph(ICommandDataModuleGraph<T> commandDataModuleGraph, IDataModuleGraphConditions<T> conditions)
        {
            _commandDataModuleGraph = commandDataModuleGraph;
            _conditions = conditions;
        }
        
        public bool Contains(IModule module)
        {
            return _commandDataModuleGraph.Contains(module);
        }

        public bool Contains(IConnection connection)
        {
            return _commandDataModuleGraph.Contains(connection);
        }

        public void Add(IDataModule<T> module)
        {
            if(CanAdd(module)) _commandDataModuleGraph.Add(module);
        }

        public void Remove(IDataModule<T> module)
        {
            if(CanRemove(module)) _commandDataModuleGraph.Remove(module);
        }

        public IDataConnection<T> Connect(IDataPort<T> port1, IDataPort<T> port2)
        {
            return CanConnect(port1,port2) ? _commandDataModuleGraph.Connect(port1, port2) : null;
        }

        public void Disconnect(IDataConnection<T> connection)
        {
            if(CanDisconnect(connection)) _commandDataModuleGraph.Disconnect(connection);
        }

        public void AddAndConnect(IDataModule<T> module, IDataPort<T> ownerPort, IDataPort<T> targetPort)
        {
            if (CanAddAndConnect(module, ownerPort, targetPort)) _commandDataModuleGraph.AddAndConnect(module, ownerPort, targetPort);
        }

        public bool CanAdd(IDataModule<T> module) => _conditions.CanAdd(module);
        public bool CanRemove(IDataModule<T> module) => _conditions.CanRemove(module);
        public bool CanConnect(IDataPort<T> output, IDataPort<T> input) => _conditions.CanConnect(output,input);
        public bool CanDisconnect(IDataConnection<T> connection) => _conditions.CanDisconnect(connection);
        public bool CanAddAndConnect(IDataModule<T> module, IDataPort<T> ownerPort, IDataPort<T> targetPort) => _conditions.CanAddAndConnect(module,ownerPort,targetPort);
        public int CommandsToUndoCount => _commandDataModuleGraph.CommandsToUndoCount;
        public int CommandsToRedoCount => _commandDataModuleGraph.CommandsToRedoCount;
        public bool CanUndo(int count) => _commandDataModuleGraph.CanUndo(count);
        public bool CanRedo(int count) => _commandDataModuleGraph.CanRedo(count);
        public void Undo(int count) => _commandDataModuleGraph.Undo(count);
        public void Redo(int count) => _commandDataModuleGraph.Redo(count);
    }
}