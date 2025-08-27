namespace IM.Graphs
{
    public interface IGraph : IGraphReadOnly
    {
        IEdge Connect(INode first, INode second);
        INode CreateNode();
        void Disconnect(IEdge edge);
        void RemoveNode(INode node);
    }
}