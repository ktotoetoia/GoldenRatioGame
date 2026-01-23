using System;
using System.Collections.Generic;

namespace IM.Graphs
{
    public class AccessConditionalCommandModuleGraph : IConditionalCommandModuleGraph, IModuleGraphAccess
    {
        private readonly IConditionalCommandModuleGraph _graph;

        public bool CanUse { get; set; }
        public bool ThrowIfCantUse { get; set; }

        public IEnumerable<IConnection> Connections => _graph.Connections;
        public IEnumerable<IModule> Modules => _graph.Modules;
        public IEnumerable<INode> Nodes => _graph.Nodes;
        public IEnumerable<IEdge> Edges => _graph.Edges;

        public int CommandsToUndoCount => _graph.CommandsToUndoCount;
        public int CommandsToRedoCount => _graph.CommandsToRedoCount;

        public AccessConditionalCommandModuleGraph()
            : this(new ConditionalCommandModuleGraph())
        {
        }

        public AccessConditionalCommandModuleGraph(IConditionalCommandModuleGraph graph)
        {
            _graph = graph ?? throw new ArgumentNullException(nameof(graph));
        }

        public void AddModule(IModule module)
        {
            if (!TryUse()) return;
            _graph.AddModule(module);
        }

        public void RemoveModule(IModule module)
        {
            if (!TryUse()) return;
            _graph.RemoveModule(module);
        }

        public IConnection Connect(IPort output, IPort input)
        {
            if (!TryUse()) return null;
            return _graph.Connect(output, input);
        }

        public void Disconnect(IConnection connection)
        {
            if (!TryUse()) return;
            _graph.Disconnect(connection);
        }

        public void AddAndConnect(IModule module, IPort ownerPort, IPort targetPort)
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

        public bool CanUndo(int count) => _graph.CanUndo(count);
        public bool CanRedo(int count) => _graph.CanRedo(count);
        public bool CanAddModule(IModule module) => _graph.CanAddModule(module);
        public bool CanRemoveModule(IModule module) => _graph.CanRemoveModule(module);
        public bool CanConnect(IPort output, IPort input) => _graph.CanConnect(output, input);
        public bool CanDisconnect(IConnection connection) => _graph.CanDisconnect(connection);
        public bool CanAddAndConnect(IModule module, IPort ownerPort, IPort targetPort)
            => _graph.CanAddAndConnect(module, ownerPort, targetPort);

        private bool TryUse()
        {
            if (CanUse) return true;

            if (ThrowIfCantUse)
                throw new InvalidOperationException("Graph access denied (CanUse = false).");

            return false;
        }
        public bool Contains(IModule module)
        {
            return _graph.Contains(module);
        }

        public bool Contains(IConnection connection)
        {
            return _graph.Contains(connection);
        }
    }
}