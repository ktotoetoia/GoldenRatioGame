using System.Collections;
using System.Collections.Generic;

namespace IM.Graphs
{
    public class Node<T> : INode<T>, ICollection<IEdge<T>>
    {
        private readonly List<IEdge<T>> _edges = new();

        public T Value { get; set; }
        public IEnumerable<IEdge<T>> Edges => _edges;
        public int Count => _edges.Count;
        public bool IsReadOnly => false;

        public Node() : this(default) { }

        public Node(T value)
        {
            Value = value;
        }

        public void Add(IEdge<T> edge)
        {
            _edges.Add(edge);
        }

        public bool Remove(IEdge<T> edge)
        {
            return _edges.Remove(edge);
        }

        public void Clear()
        {
            _edges.Clear();
        }

        public bool Contains(IEdge<T> item)
        {
            return _edges.Contains(item);
        }

        public void CopyTo(IEdge<T>[] array, int arrayIndex)
        {
            _edges.CopyTo(array, arrayIndex);
        }

        public IEnumerator<IEdge<T>> GetEnumerator()
        {
            return _edges.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _edges.GetEnumerator();
        }
    }
}