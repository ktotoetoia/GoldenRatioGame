using System;
using System.Collections.Generic;
using IM.LifeCycle;

namespace IM.Graphs
{
    public class AccessConditionalCommandDataModuleGraph<T> : IConditionalCommandDataModuleGraph<T>, IAccess
    {
        private readonly IConditionalCommandDataModuleGraph<T> _graph;

        public bool CanUse { get; set; }
        public bool ThrowIfCantUse { get; set; }

        public int CommandsToUndoCount => _graph.CommandsToUndoCount;
        public int CommandsToRedoCount => _graph.CommandsToRedoCount;
        public IEnumerable<INode> Nodes => _graph.Nodes;
        public IEnumerable<IEdge> Edges => _graph.Edges;
        public IReadOnlyList<IModule> Modules => _graph.Modules;
        public IReadOnlyList<IConnection> Connections => _graph.Connections;
        public IEnumerable<IDataModule<T>> DataModules => _graph.DataModules;
        public IEnumerable<IDataConnection<T>> DataConnections => _graph.DataConnections;

        public AccessConditionalCommandDataModuleGraph(IConditionalCommandDataModuleGraph<T> graph)
        {
            _graph = graph ?? throw new ArgumentNullException(nameof(graph));
        }

        private bool TryUse()
        {
            if (CanUse) return true;
            
            return ThrowIfCantUse ? throw new InvalidOperationException("Graph access denied (CanUse = false).") : false;
        }

        public void Add(IDataModule<T> module)
        {
            if (!TryUse()) return;
            _graph.Add(module);
        }

        public void Remove(IDataModule<T> module)
        {
            if (!TryUse()) return;
            _graph.Remove(module);
        }

        public IDataConnection<T> Connect(IDataPort<T> port1, IDataPort<T> port2)
        {
            if (!TryUse()) return null;
            return _graph.Connect(port1, port2);
        }

        public void Disconnect(IDataConnection<T> connection)
        {
            if (!TryUse()) return;
            _graph.Disconnect(connection);
        }

        public void AddAndConnect(IDataModule<T> module, IDataPort<T> ownerPort, IDataPort<T> targetPort)
        {
            if (!TryUse()) return;
            _graph.AddAndConnect(module, ownerPort, targetPort);
        }

        public void Undo(int count)
        {
            if (!TryUse()) return;
            _graph.Undo(count);
        }

        public void Redo(int count)
        {
            if (!TryUse()) return;
            _graph.Redo(count);
        }

        public bool Contains(IModule module) => _graph.Contains(module);
        public bool Contains(IConnection connection) => _graph.Contains(connection);
        public bool CanUndo(int count) => _graph.CanUndo(count);
        public bool CanRedo(int count) => _graph.CanRedo(count);
        public bool CanAdd(IDataModule<T> module) => _graph.CanAdd(module);
        public bool CanRemove(IDataModule<T> module) => _graph.CanRemove(module);
        public bool CanConnect(IDataPort<T> port1, IDataPort<T> port2) => _graph.CanConnect(port1, port2);
        public bool CanAddAndConnect(IDataModule<T> module, IDataPort<T> ownerPort, IDataPort<T> targetPort) =>
            _graph.CanAddAndConnect(module, ownerPort, targetPort);
        public bool CanDisconnect(IDataConnection<T> connection) => _graph.CanDisconnect(connection);
    }
}