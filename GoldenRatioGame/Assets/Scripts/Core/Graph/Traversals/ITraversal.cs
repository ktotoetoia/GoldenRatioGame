namespace IM.Graphs
{
    public interface ITraversal
    {
        IGraph GetSubGraph(INode node);
    }
}