using System.Collections;
using System.Collections.Generic;

namespace IM.Graphs
{
    public class DataNode<T> : IDataNode<T>, ICollection<IDataEdge<T>>
    {
        private readonly List<IDataEdge<T>>  _edges = new();

        public DataNode(T value)
        {
            Value = value;
        }
        
        public T Value { get; set; }
        public IEnumerable<IEdge> Edges => DataEdges;
        public IEnumerable<IDataEdge<T>> DataEdges => _edges;
        public IEnumerator<IDataEdge<T>> GetEnumerator() => _edges.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_edges).GetEnumerator();
        public void Add(IDataEdge<T> item) => _edges.Add(item);
        public void Clear() => _edges.Clear();
        public bool Contains(IDataEdge<T> item) => _edges.Contains(item);
        public void CopyTo(IDataEdge<T>[] array, int arrayIndex) => _edges.CopyTo(array, arrayIndex);
        public bool Remove(IDataEdge<T> item) => _edges.Remove(item);
        public int Count => _edges.Count;
        public bool IsReadOnly => false;
    }
}