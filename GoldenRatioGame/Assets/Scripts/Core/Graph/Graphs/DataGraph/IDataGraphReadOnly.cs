using System.Collections.Generic;

namespace IM.Graphs
{
    public interface IDataGraphReadOnly<T> : IGraphReadOnly
    {
        IEnumerable<IDataNode<T>> DataNodes { get; }
        IEnumerable<IDataEdge<T>> DataEdges { get; }
    }
}