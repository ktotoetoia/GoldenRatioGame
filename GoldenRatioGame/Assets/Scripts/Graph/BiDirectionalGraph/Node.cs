using System.Collections;
using System.Collections.Generic;

namespace IM.Graphs
{
    public class Node: INode, ICollection<IEdge>
    {
        private readonly List<IEdge> _edges = new();

        public IEnumerable<IEdge> Edges => _edges;
        public int Count => _edges.Count;
        public bool IsReadOnly => false;

        public Node()
        {
            
        }

        public void Add(IEdge edge)
        {
            _edges.Add(edge);
        }

        public bool Remove(IEdge edge)
        {
            return _edges.Remove(edge);
        }

        public void Clear()
        {
            _edges.Clear();
        }

        public bool Contains(IEdge item)
        {
            return _edges.Contains(item);
        }

        public void CopyTo(IEdge[] array, int arrayIndex)
        {
            _edges.CopyTo(array, arrayIndex);
        }

        public IEnumerator<IEdge> GetEnumerator()
        {
            return _edges.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _edges.GetEnumerator();
        }
    }
}