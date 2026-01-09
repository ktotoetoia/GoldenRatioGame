namespace IM.Graphs
{
    public interface IEdge
    {
        INode Node1 { get; }
        INode Node2 { get; }
    }
}