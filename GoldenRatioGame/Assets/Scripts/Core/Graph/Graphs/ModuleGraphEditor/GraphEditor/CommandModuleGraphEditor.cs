using System;
using UnityEngine;

namespace IM.Graphs
{
    public class CommandModuleGraphEditor : IModuleGraphEditor<ICommandModuleGraph>
    {
        private readonly ICommandModuleGraph _graph;
        private readonly IModuleGraphValidator _validator;
        private readonly IModuleGraphObserver _observer;
        private IAccessCommandModuleGraph _accessModuleGraph;
        private int _undoIndexAtEditStart;

        public bool IsEditing => _accessModuleGraph != null;
        public bool CanSaveChanges => IsEditing && _validator.IsValid(_accessModuleGraph);
        public IModuleGraphReadOnly Graph { get; }

        public CommandModuleGraphEditor() : this(new CommandModuleGraph())
        {
            
        }
        
        public CommandModuleGraphEditor(ICommandModuleGraph graph) : this(graph, new TrueModuleGraphValidator(), new EmptyObserver())
        {
            
        }

        public CommandModuleGraphEditor(ICommandModuleGraph graph, IModuleGraphValidator moduleGraphValidator, IModuleGraphObserver observer)
        {
            _graph = graph ?? throw new ArgumentNullException(nameof(graph));
            _validator = moduleGraphValidator  ?? throw new ArgumentNullException(nameof(moduleGraphValidator));
            _observer = observer;
            
            Graph = new ModuleGraphReadOnlyWrapper(_graph);
        }
        
        public ICommandModuleGraph StartEditing()
        {
            if (IsEditing) throw new InvalidOperationException("Graph is already being edited");

            _undoIndexAtEditStart = _graph.CommandsToUndoCount;
            
            return _accessModuleGraph = new AccessCommandModuleGraph(_graph) {CanUse = true, ThrowIfCantUse = true};
        }

        public void CancelChanges()
        {
            if(!IsEditing) throw new InvalidOperationException("Graph is not being edited");
            
            _graph.Undo(_graph.CommandsToUndoCount - _undoIndexAtEditStart);
            
            EndEditing();
        }

        public bool TrySaveChanges()
        {
            if(!IsEditing) throw new InvalidOperationException("Graph is not being edited");

            if (CanSaveChanges || _validator.TryFix(_accessModuleGraph))
            {
                EndEditing();

                _observer.Update(Graph);
                
                return true;
            }

            return false;
        }
        
        private void EndEditing()
        {
            _accessModuleGraph.CanUse = false;
            _accessModuleGraph = null;
            
            if (_graph is CommandModuleGraph cmd)
            {
                cmd.ClearUndoCommands();
                cmd.ClearRedoCommands();
            }
        }
    }
}