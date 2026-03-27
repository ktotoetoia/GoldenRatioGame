using System.Collections.Generic;

namespace IM.Graphs
{
    public interface IDataGraph<T> : IDataGraphReadOnly<T>
    {
        IDataNode<T> Create(T data);
        IDataEdge<T> Connect(IDataNode<T> dataNode1, IDataNode<T> dataNode2);
        void Disconnect(IDataEdge<T> edge);
        IEnumerable<IDataNode<T>> RemoveAll(T data);
        void Remove(IDataNode<T> node);
    }
}