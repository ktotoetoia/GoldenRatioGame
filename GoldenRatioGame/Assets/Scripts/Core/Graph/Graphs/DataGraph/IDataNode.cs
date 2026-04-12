using System.Collections.Generic;

namespace IM.Graphs
{
    public interface IDataNode<T>  :INode, IHaveNodeValue<T>
    {
        IEnumerable<IDataEdge<T>> DataEdges { get; }
    }
}