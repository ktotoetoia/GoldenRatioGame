using System.Collections.Generic;

namespace IM.Graphs
{
    public interface IDataNode<T>  :INode
    {
        public T Value { get; set; }
        IEnumerable<IDataEdge<T>> DataEdges { get; }
    }
}