namespace IM.Graphs
{
    public class DataEdge<T> : IDataEdge<T>
    {
        public INode Node1 => DataNode1;
        public INode Node2 => DataNode2;
        public IDataNode<T> DataNode1 { get; }
        public IDataNode<T> DataNode2 { get; }
    
        public DataEdge(IDataNode<T> dataNode1, IDataNode<T> dataNode2)
        {
            DataNode1 = dataNode1;
            DataNode2 = dataNode2;
        }
    }
}