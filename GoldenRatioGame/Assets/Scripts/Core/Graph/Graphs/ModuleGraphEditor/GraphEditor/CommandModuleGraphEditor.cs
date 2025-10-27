using System;
using IM.Base;

namespace IM.Graphs
{
    public class CommandModuleGraphEditor<TGraph> : IModuleGraphEditor<TGraph>
        where TGraph : class, ICommandModuleGraph
    {
        private readonly TGraph _graph;
        private readonly IModuleGraphValidator _validator;
        private readonly IModuleGraphObserver _observer;
        private readonly IFactory<IModuleGraphAccess, TGraph> _accessGraphFactory;

        private IModuleGraphAccess _accessModuleGraph;
        private int _undoIndexAtEditStart;

        public bool IsEditing => _accessModuleGraph != null;
        public bool CanSaveChanges => IsEditing && _validator.IsValid(Graph);
        public IModuleGraphReadOnly Graph { get; }
        
        public CommandModuleGraphEditor(TGraph graph, IFactory<IModuleGraphAccess, TGraph> accessGraphFactory)
            : this(graph, accessGraphFactory, new TrueModuleGraphValidator(), new EmptyObserver())
        {
            
        }

        public CommandModuleGraphEditor(
            TGraph graph,
            IFactory<IModuleGraphAccess, TGraph> accessGraphFactory,
            IModuleGraphValidator validator,
            IModuleGraphObserver observer
            )
        {
            _graph = graph ?? throw new ArgumentNullException(nameof(graph));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _observer = observer ?? throw new ArgumentNullException(nameof(observer));
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
                _observer.OnGraphUpdated(Graph);
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