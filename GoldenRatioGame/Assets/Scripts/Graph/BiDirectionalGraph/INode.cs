using System.Collections.Generic;

namespace IM.Graphs
{
    public interface INode
    {
        IEnumerable<IEdge> Edges { get; }
    }
}