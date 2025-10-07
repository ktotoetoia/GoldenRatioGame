using System;
using System.Collections.Generic;

namespace IM.Graphs
{
    public class AccessCommandModuleGraph : IAccessCommandModuleGraph
    {
        private readonly ICommandModuleGraph _graph;

        public IReadOnlyList<INode> Nodes => _graph.Nodes;
        public IReadOnlyList<IEdge> Edges => _graph.Edges;
        public IReadOnlyList<IConnection> Connections => _graph.Connections;
        public IReadOnlyList<IModule> Modules => _graph.Modules;

        public int CommandsToUndoCount => _graph.CommandsToUndoCount;
        public int CommandsToRedoCount => _graph.CommandsToRedoCount;

        public bool CanUse { get; set; }
        public bool ThrowIfCantUse { get; set; }

        public AccessCommandModuleGraph() : this(new CommandModuleGraph()) { }

        public AccessCommandModuleGraph(ICommandModuleGraph graph)
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

        public IConnection Connect(IModulePort output, IModulePort input)
        {
            if (!TryUse()) return null;
            return _graph.Connect(output, input);
        }

        public void Disconnect(IConnection connection)
        {
            if (!TryUse()) return;
            _graph.Disconnect(connection);
        }

        public void Undo(int count)
        {
            if (!TryUse()) return;
            if (CanUndo(count))
                _graph.Undo(count);
        }

        public void Redo(int count)
        {
            if (!TryUse()) return;
            if (CanRedo(count))
                _graph.Redo(count);
        }

        private bool TryUse()
        {
            if (!CanUse && ThrowIfCantUse)
                throw new InvalidOperationException("Cannot use this graph.");
            return CanUse;
        }

        public bool CanUndo(int count) => _graph.CommandsToUndoCount >= count;
        public bool CanRedo(int count) => _graph.CommandsToRedoCount >= count;
    }
}