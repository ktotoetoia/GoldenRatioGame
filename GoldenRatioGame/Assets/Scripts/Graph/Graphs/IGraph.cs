namespace IM.Graphs
{
    public interface IGraph<T> : IGraphReadOnly<T>
    {
        IEdge<T> Connect(INode<T> first, INode<T> second);
        INode<T> CreateNode();
        void Disconnect(IEdge<T> edge);
        void RemoveNode(INode<T> node);
    }
}