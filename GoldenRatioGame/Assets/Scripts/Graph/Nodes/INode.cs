using System.Collections.Generic;

namespace IM.Graphs
{
    public interface INode<T> :IValue<T>
    {
        IEnumerable<IEdge<T>> Edges { get; }
    }
}