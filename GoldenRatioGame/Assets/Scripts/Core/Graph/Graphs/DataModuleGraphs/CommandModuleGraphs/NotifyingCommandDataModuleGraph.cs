using System;
using System.Collections.Generic;

namespace IM.Graphs
{
    public class NotifyingCommandDataModuleGraph<T> : ICommandDataModuleGraph<T>, INotifyOnEditingEnded
    {
        private readonly ICommandDataModuleGraph<T> _innerGraph;
        public event Action OnGraphChanged;

        public NotifyingCommandDataModuleGraph() : this(new CommandDataModuleGraph<T>())
        {
            
        }
        
        public NotifyingCommandDataModuleGraph(ICommandDataModuleGraph<T> innerGraph)
        {
            _innerGraph = innerGraph ?? throw new ArgumentNullException(nameof(innerGraph));
        }

        private void Notify() => OnGraphChanged?.Invoke();

        public void Add(IDataModule<T> toAdd)
        {
            _innerGraph.Add(toAdd);
            Notify();
        }

        public void Remove(IDataModule<T> module)
        {
            _innerGraph.Remove(module);
            Notify();
        }

        public IDataConnection<T> Connect(IDataPort<T> port1, IDataPort<T> port2)
        {
            var connection = _innerGraph.Connect(port1, port2);
            Notify();
            return connection;
        }

        public void Disconnect(IDataConnection<T> connection)
        {
            _innerGraph.Disconnect(connection);
            Notify();
        }

        public void AddAndConnect(IDataModule<T> module, IDataPort<T> ownerPort, IDataPort<T> targetPort)
        {
            _innerGraph.AddAndConnect(module, ownerPort, targetPort);
            Notify();
        }

        public void Undo(int count)
        {
            _innerGraph.Undo(count);
            Notify();
        }

        public void Redo(int count)
        {
            _innerGraph.Redo(count);
            Notify();
        }

        public void OnEditingEnded()
        {
            if (_innerGraph is INotifyOnEditingEnded notifier)
            {
                notifier.OnEditingEnded();
            }
            Notify();
        }
        
        public IEnumerable<INode> Nodes => _innerGraph.Nodes;
        public IReadOnlyList<IModule> Modules => _innerGraph.Modules;
        public IEnumerable<IDataModule<T>> DataModules => _innerGraph.DataModules;
        public IEnumerable<IEdge> Edges => _innerGraph.Edges;
        public IReadOnlyList<IConnection> Connections => _innerGraph.Connections;
        public IEnumerable<IDataConnection<T>> DataConnections => _innerGraph.DataConnections;
        public int CommandsToUndoCount => _innerGraph.CommandsToUndoCount;
        public int CommandsToRedoCount => _innerGraph.CommandsToRedoCount;

        public bool Contains(IModule module) => _innerGraph.Contains(module);
        public bool Contains(IConnection connection) => _innerGraph.Contains(connection);
        public bool CanUndo(int count) => _innerGraph.CanUndo(count);
        public bool CanRedo(int count) => _innerGraph.CanRedo(count);
    }
}