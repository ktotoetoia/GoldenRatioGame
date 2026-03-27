namespace IM.Graphs
{
    public interface IDataEdge<T> : IEdge
    {
        IDataNode<T> DataNode1 { get; }
        IDataNode<T> DataNode2 { get; }
    }
}