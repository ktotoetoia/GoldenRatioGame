using System.Collections.Generic;

namespace IM.Graphs
{
    public class GraphReadOnly : IGraphReadOnly
    {
        public IEnumerable<INode> Nodes { get; }
        public IEnumerable<IEdge> Edges { get; }
        
        public GraphReadOnly(IReadOnlyList<INode> nodes, IReadOnlyList<IEdge> edges)
        {
            Nodes = nodes;
            Edges = edges;
        }
    }
}