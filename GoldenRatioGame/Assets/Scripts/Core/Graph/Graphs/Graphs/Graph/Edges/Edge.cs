namespace IM.Graphs
{
    public class Edge : IEdge
    {
        public INode Node1 { get; }
        public INode Node2 { get; }

        public Edge(INode from, INode to)
        {
            Node1 = from;
            Node2 = to;
        }
    }
}