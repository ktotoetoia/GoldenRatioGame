using System;
using System.Collections.Generic;
using IM.Common;

namespace IM.Graphs
{
    public class CommandModuleGraphEditor<TGraph> : IModuleGraphEditor<TGraph>
        where TGraph : class, ICommandModuleGraph
    {
        private readonly TGraph _graph;
        private readonly IModuleGraphValidator _validator;
        private readonly IFactory<IAccess, TGraph> _accessGraphFactory;
        private readonly List<IEditorObserver<IModuleGraphReadOnly>> _observers = new();
        private IAccess _access;
        private int _undoIndexAtEditStart;

        public bool IsEditing => _access != null;
        public bool CanSaveChanges => IsEditing && _validator.IsValid(Snapshot);
        public IModuleGraphReadOnly Snapshot { get; }
        public ICollection<IEditorObserver<IModuleGraphReadOnly>> Observers => _observers;
        
        public CommandModuleGraphEditor(TGraph graph, IFactory<IAccess, TGraph> accessGraphFactory)
            : this(graph, accessGraphFactory, new TrueModuleGraphValidator())
        {
            
        }

        public CommandModuleGraphEditor(
            TGraph graph,
            IFactory<IAccess, TGraph> accessGraphFactory,
            IModuleGraphValidator validator
            )
        {
            _graph = graph ?? throw new ArgumentNullException(nameof(graph));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _accessGraphFactory = accessGraphFactory ?? throw new ArgumentNullException(nameof(accessGraphFactory));

            Snapshot = new ModuleGraphReadOnlyWrapper(_graph);
        }

        public TGraph BeginEdit()
        {
            if (IsEditing)
                throw new InvalidOperationException("Graph is already being edited.");

            _undoIndexAtEditStart = _graph.CommandsToUndoCount;
            _access = _accessGraphFactory.Create(_graph);
            
            return (TGraph)_access;
        }

        public void DiscardChanges()
        {
            if (!IsEditing)
                throw new InvalidOperationException("Graph is not being edited.");

            _graph.Undo(_graph.CommandsToUndoCount - _undoIndexAtEditStart);
            EndEditing();
        }

        public bool TryApplyChanges()
        {
            if (!IsEditing)
                throw new InvalidOperationException("Graph is not being edited.");

            if (!CanSaveChanges && !_validator.TryFix(_graph)) return false;

            EndEditing();
            _observers.ForEach(x => x.OnSnapshotChanged(Snapshot));
            return true;
        }
        
        private void EndEditing()
        {
            _access.CanUse = false;
            _access = null;

            if (_graph is INotifyOnEditingEnded ntf)
            {
                ntf.OnEditingEnded();
            }
        }
    }
}