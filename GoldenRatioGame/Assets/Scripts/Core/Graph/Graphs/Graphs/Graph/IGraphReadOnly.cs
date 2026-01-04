using System.Collections.Generic;

namespace IM.Graphs
{
    public interface IGraphReadOnly
    {
        IEnumerable<INode> Nodes { get; }
        IEnumerable<IEdge> Edges { get; }
    }
}