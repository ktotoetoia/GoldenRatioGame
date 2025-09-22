namespace IM.Graphs
{
    public class Edge : IEdge
    {
        public INode From { get; }
        public INode To { get; }

        public Edge(INode from, INode to)
        {
            From = from;
            To = to;
        }
    }
}