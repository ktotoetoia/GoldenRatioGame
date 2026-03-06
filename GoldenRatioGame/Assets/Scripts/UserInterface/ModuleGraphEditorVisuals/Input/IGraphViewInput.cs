using IM.Graphs;

namespace IM.Visuals.Graph
{
    public interface IGraphViewInput
    {
        void SetGraph(IConditionalCommandModuleGraph graph);
        void ClearGraph();
    }
}