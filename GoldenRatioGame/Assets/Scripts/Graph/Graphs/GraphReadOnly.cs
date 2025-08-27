using System.Collections.Generic;

namespace IM.Graphs
{
    public class GraphReadOnly<T> : IGraphReadOnly<T>
    {
        public IReadOnlyList<INode<T>> Nodes { get; }
        public IReadOnlyList<IEdge<T>> Edges { get; }
        
        public GraphReadOnly(IReadOnlyList<INode<T>> nodes, IReadOnlyList<IEdge<T>> edges)
        {
            Nodes = nodes;
            Edges = edges;
        }
    }
}