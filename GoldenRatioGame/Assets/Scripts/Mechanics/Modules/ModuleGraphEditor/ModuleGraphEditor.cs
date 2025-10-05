using System;
using IM.Graphs;

namespace IM.Modules
{
    public class ModuleGraphEditor : IModuleGraphEditor
    {
        private readonly ICommandModuleGraph _graph;
        private readonly IModuleGraphValidator _moduleGraphValidator;
        private IAccessModuleGraph _accessModuleGraph;
        private int _undoIndexAtEditStart;

        public IModuleGraphReadOnly Graph { get; }

        public bool IsEditing => _accessModuleGraph != null;
        public bool CanSaveChanges => IsEditing && _moduleGraphValidator.IsValid(_accessModuleGraph);

        public ModuleGraphEditor() : this(new CommandModuleGraph())
        {
            
        }
        
        public ModuleGraphEditor(ICommandModuleGraph graph) : this(graph, new TrueModuleGraphValidator())
        {
            
        }

        public ModuleGraphEditor(ICommandModuleGraph graph, IModuleGraphValidator moduleGraphValidator)
        {
            _graph = graph ?? throw new ArgumentNullException(nameof(graph));
            _moduleGraphValidator = moduleGraphValidator  ?? throw new ArgumentNullException(nameof(moduleGraphValidator));
            
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

            if (CanSaveChanges)
            {
                EndEditing();
                
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