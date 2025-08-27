using System.Collections.Generic;

namespace IM.Graphs
{
    public class GraphReadOnly : IGraphReadOnly
    {
        public IReadOnlyList<INode> Nodes { get; }
        public IReadOnlyList<IEdge> Edges { get; }
        
        public GraphReadOnly(IReadOnlyList<INode> nodes, IReadOnlyList<IEdge> edges)
        {
            Nodes = nodes;
            Edges = edges;
        }
    }
}