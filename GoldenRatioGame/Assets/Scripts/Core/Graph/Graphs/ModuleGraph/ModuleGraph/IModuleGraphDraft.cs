namespace IM.Graphs
{
    public interface IModuleGraphDraft : IModuleGraph
    {
        void ApplyChangesToMainGraph();
    }
}