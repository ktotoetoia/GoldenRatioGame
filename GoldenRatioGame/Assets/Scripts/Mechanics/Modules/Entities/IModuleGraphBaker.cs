using IM.Graphs;

namespace IM.Modules
{
    public interface IModuleGraphBaker
    {
        bool IsBaking { get; }
        IModuleGraphReadOnly Graph { get; }

        IModuleGraph StartBaking();
        void CancelBaking();
        void FinishBaking();
    }

    public class ModuleGraphBaker : IModuleGraphBaker
    {
        private readonly ModuleGraph _graph;
        private IModuleGraphDraft _bakingGraph;

        public bool IsBaking => _bakingGraph != null;
        public IModuleGraphReadOnly Graph => _graph;

        public IModuleGraph StartBaking()
        {
            if (IsBaking)
            {
                throw new System.InvalidOperationException("Baking is already in progress");
            }

            _bakingGraph = new ModuleGraphDraft(_graph);

            return _bakingGraph;
        }

        public void CancelBaking()
        {
            _bakingGraph = null;
        }

        public void FinishBaking()
        {
            _bakingGraph.ApplyChangesToMainGraph();
            _bakingGraph = null;
        }
    }
}