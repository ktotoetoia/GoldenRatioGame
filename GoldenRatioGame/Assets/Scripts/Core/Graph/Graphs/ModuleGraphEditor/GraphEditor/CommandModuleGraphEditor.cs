using System;
using System.Collections.Generic;
using IM.Base;

namespace IM.Graphs
{
    public class CommandModuleGraphEditor<TGraph> : IModuleGraphEditor<TGraph>
        where TGraph : class, ICommandModuleGraph
    {
        private readonly TGraph _graph;
        private readonly IModuleGraphValidator _validator;
        private readonly IFactory<IModuleGraphAccess, TGraph> _accessGraphFactory;
        private readonly List<IModuleGraphObserver> _observers = new();
        private IModuleGraphAccess _accessModuleGraph;
        private int _undoIndexAtEditStart;

        public bool IsEditing => _accessModuleGraph != null;
        public bool CanSaveChanges => IsEditing && _validator.IsValid(Graph);
        public IModuleGraphReadOnly Graph { get; }
        public IEnumerable<IModuleGraphObserver> Observers => _observers;
        
        public CommandModuleGraphEditor(TGraph graph, IFactory<IModuleGraphAccess, TGraph> accessGraphFactory)
            : this(graph, accessGraphFactory, new TrueModuleGraphValidator())
        {
            
        }

        public CommandModuleGraphEditor(
            TGraph graph,
            IFactory<IModuleGraphAccess, TGraph> accessGraphFactory,
            IModuleGraphValidator validator
            )
        {
            _graph = graph ?? throw new ArgumentNullException(nameof(graph));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _accessGraphFactory = accessGraphFactory ?? throw new ArgumentNullException(nameof(accessGraphFactory));

            Graph = new ModuleGraphReadOnlyWrapper(_graph);
        }

        public TGraph StartEditing()
        {
            if (IsEditing)
                throw new InvalidOperationException("Graph is already being edited.");

            _undoIndexAtEditStart = _graph.CommandsToUndoCount;
            _accessModuleGraph = _accessGraphFactory.Create(_graph);
            
            return (TGraph)_accessModuleGraph;
        }

        public void CancelChanges()
        {
            if (!IsEditing)
                throw new InvalidOperationException("Graph is not being edited.");

            _graph.Undo(_graph.CommandsToUndoCount - _undoIndexAtEditStart);
            EndEditing();
        }

        public bool TrySaveChanges()
        {
            if (!IsEditing)
                throw new InvalidOperationException("Graph is not being edited.");

            if (CanSaveChanges || _validator.TryFix(_graph))
            {
                EndEditing();
                _observers.ForEach(x => x.OnGraphUpdated(Graph));
                return true;
            }

            return false;
        }
        
        public void AddObserver(IModuleGraphObserver observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }

        public void RemoveObserver(IModuleGraphObserver observer)
        {
            _observers.Remove(observer);
        }

        private void EndEditing()
        {
            _accessModuleGraph.CanUse = false;
            _accessModuleGraph = null;

            if (_graph is INotifyOnEditingEnded ntf)
            {
                ntf.OnEditingEnded();
            }
        }
    }
}