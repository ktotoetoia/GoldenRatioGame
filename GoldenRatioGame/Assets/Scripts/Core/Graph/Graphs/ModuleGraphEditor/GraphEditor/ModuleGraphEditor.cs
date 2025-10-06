using System;

namespace IM.Graphs
{
    public class ModuleGraphEditor : IModuleGraphEditor
    {
        private readonly ICommandModuleGraph _graph;
        private readonly IModuleGraphValidator _validator;
        private readonly IModuleGraphObserver _observer;
        private IAccessModuleGraph _accessModuleGraph;
        private int _undoIndexAtEditStart;

        public bool IsEditing => _accessModuleGraph != null;
        public bool CanSaveChanges => IsEditing && _validator.IsValid(_accessModuleGraph);
        public IModuleGraphReadOnly Graph { get; }


        public ModuleGraphEditor() : this(new CommandModuleGraph())
        {
            
        }
        
        public ModuleGraphEditor(ICommandModuleGraph graph) : this(graph, new TrueModuleGraphValidator(), new EmptyObserver())
        {
            
        }

        public ModuleGraphEditor(ICommandModuleGraph graph, IModuleGraphValidator moduleGraphValidator, IModuleGraphObserver observer)
        {
            _graph = graph ?? throw new ArgumentNullException(nameof(graph));
            _validator = moduleGraphValidator  ?? throw new ArgumentNullException(nameof(moduleGraphValidator));
            _observer = observer;
            
            Graph = new ModuleGraphReadOnlyWrapper(_graph);
        }
        
        public IModuleGraph StartEditing()
        {
            if (IsEditing) throw new InvalidOperationException("Graph is already being edited");

            _undoIndexAtEditStart = _graph.CommandsToUndoCount;
            
            return _accessModuleGraph = new AccessModuleGraph(_graph) {CanUse = true, ThrowIfCantUse = true};
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
        }
    }
}