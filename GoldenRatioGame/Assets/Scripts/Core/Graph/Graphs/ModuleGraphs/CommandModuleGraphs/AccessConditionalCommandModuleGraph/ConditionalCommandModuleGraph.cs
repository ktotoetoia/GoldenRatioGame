using System;
using System.Collections.Generic;
using UnityEngine;

namespace IM.Graphs
{
    public class ConditionalCommandModuleGraph : IConditionalCommandModuleGraph, INotifyOnEditingEnded
    {
        private readonly ICommandModuleGraph _graph;
        private readonly IModuleGraphConditions _conditions;

        public IReadOnlyList<IConnection> Connections => _graph.Connections;

        public IReadOnlyList<IModule> Modules => _graph.Modules;
        public IEnumerable<INode> Nodes => _graph.Nodes;
        public IEnumerable<IEdge> Edges => _graph.Edges;
        public int CommandsToUndoCount => _graph.CommandsToUndoCount;
        public int CommandsToRedoCount => _graph.CommandsToRedoCount;

        public ConditionalCommandModuleGraph() : this(new CommandModuleGraph())
        {
            
        }
        
        public ConditionalCommandModuleGraph(ICommandModuleGraph graph) : this(graph, new DefaultModuleGraphConditions(graph)) { }

        public ConditionalCommandModuleGraph(ICommandModuleGraph graph, IModuleGraphConditions conditions)
        {
            _graph = graph ?? throw new ArgumentNullException(nameof(graph));
            _conditions = conditions ?? throw new ArgumentNullException(nameof(conditions));
        }

        public void AddModule(IModule module)
        {
            if (!_conditions.CanAddModule(module))
                throw new InvalidOperationException("Cannot add module (condition failed).");

            _graph.AddModule(module);
        }

        public void RemoveModule(IModule module)
        {
            if (!_conditions.CanRemoveModule(module))
                throw new InvalidOperationException("Cannot remove module (condition failed).");
            
            _graph.RemoveModule(module);
        }

        public IConnection Connect(IPort output, IPort input)
        {
            if (!_conditions.CanConnect(output, input))
                throw new InvalidOperationException("Cannot connect modules (condition failed).");

            return _graph.Connect(output, input);
        }

        public void Disconnect(IConnection connection)
        {
            if (!_conditions.CanDisconnect(connection))
                throw new InvalidOperationException("Cannot disconnect (condition failed).");

            _graph.Disconnect(connection);
        }

        public void AddAndConnect(IModule module, IPort ownerPort, IPort targetPort)
        {
            if (!_conditions.CanAddAndConnect(module, ownerPort, targetPort))
                throw new InvalidOperationException("Cannot add and connect module (condition failed).");

            _graph.AddAndConnect(module, ownerPort, targetPort);
        }
        
        public bool CanAddModule(IModule module) => _conditions.CanAddModule(module);
        public bool CanRemoveModule(IModule module) => _conditions.CanRemoveModule(module);
        public bool CanConnect(IPort output, IPort input) => _conditions.CanConnect(output, input);
        public bool CanDisconnect(IConnection connection) => _conditions.CanDisconnect(connection);
        public bool CanAddAndConnect(IModule module, IPort ownerPort, IPort targetPort)
            => _conditions.CanAddAndConnect(module, ownerPort, targetPort);
        public bool CanUndo(int count) => _graph.CanUndo(count);
        public bool CanRedo(int count) => _graph.CanRedo(count);
        public void Undo(int count) => _graph.Undo(count);
        public void Redo(int count) => _graph.Redo(count);

        public void OnEditingEnded()
        {
            if (_graph is INotifyOnEditingEnded ntf)
            {
                ntf.OnEditingEnded();
            }
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