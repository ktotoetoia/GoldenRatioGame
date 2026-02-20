using IM.Graphs;

namespace IM.Visuals.Graph
{
    public interface IModuleContextInput
    {
        void SetGraph(IConditionalCommandModuleGraph graph);
        void ClearGraph();
    }
}