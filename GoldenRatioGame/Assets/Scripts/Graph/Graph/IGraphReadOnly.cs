using System.Collections.Generic;

namespace IM.Graphs
{
    public interface IGraphReadOnly
    {
        IReadOnlyList<INode> Nodes { get; }
        IReadOnlyList<IEdge> Edges { get; }
    }
}