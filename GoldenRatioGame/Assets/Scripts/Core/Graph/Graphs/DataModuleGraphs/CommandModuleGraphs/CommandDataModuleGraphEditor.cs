using System;
using System.Collections.Generic;
using IM.LifeCycle;

namespace IM.Graphs
{
    public class CommandDataModuleGraphEditor<T, TGraph> : IDataModuleGraphEditor<T, TGraph>
        where TGraph : class, IConditionalCommandDataModuleGraph<T>
    {
        private readonly TGraph _graph;
        private readonly IDataModuleGraphValidator<T> _validator;
        private readonly IFactory<IAccess, TGraph> _accessGraphFactory;
        private readonly List<IEditorObserver<IDataModuleGraphReadOnly<T>>> _observers = new();
    
        private IAccess _access;
        private int _undoIndexAtEditStart;

        public bool IsEditing => _access != null;
        public bool CanApplyChanges => IsEditing && _validator.IsValid(_graph);
        public IDataModuleGraphReadOnly<T> Snapshot { get; }
        public ICollection<IEditorObserver<IDataModuleGraphReadOnly<T>>> Observers => _observers;

        public CommandDataModuleGraphEditor(
            TGraph graph, 
            IFactory<IAccess, TGraph> accessGraphFactory)
            : this(graph, accessGraphFactory, new TrueDataModuleGraphValidator<T>())
        {
        }

        public CommandDataModuleGraphEditor(
            TGraph graph,
            IFactory<IAccess, TGraph> accessGraphFactory,
            IDataModuleGraphValidator<T> validator)
        {
            _graph = graph ?? throw new ArgumentNullException(nameof(graph));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _accessGraphFactory = accessGraphFactory ?? throw new ArgumentNullException(nameof(accessGraphFactory));

            Snapshot = new DataModuleGraphReadOnlyWrapper<T>(_graph);
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

            int commandsToUndo = _graph.CommandsToUndoCount - _undoIndexAtEditStart;
            if (commandsToUndo > 0)
            {
                _graph.Undo(commandsToUndo);
            }
        
            EndEditing();
        }

        public bool TryApplyChanges()
        {
            if (!IsEditing)
                throw new InvalidOperationException("Graph is not being edited.");

            if (!CanApplyChanges && !_validator.TryFix(_graph)) 
                return false;

            EndEditing();

            foreach (var observer in _observers)
            {
                observer.OnSnapshotChanged(Snapshot);
            }
        
            return true;
        }

        private void EndEditing()
        {
            if (_access != null)
            {
                _access.CanUse = false;
                _access = null;
            }

            if (_graph is INotifyOnEditingEnded ntf)
            {
                ntf.OnEditingEnded();
            }
        }
    }
}