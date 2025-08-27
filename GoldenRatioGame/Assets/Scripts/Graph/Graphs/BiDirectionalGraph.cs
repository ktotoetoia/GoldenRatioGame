using System;
using System.Collections.Generic;
using System.Linq;

namespace IM.Graphs
{
    public class BiDirectionalGraph<T> : IGraphReadOnly<T>
    {
        private readonly List<IEdge<T>> _edges;
        private readonly List<INode<T>> _nodes;

        public IReadOnlyList<IEdge<T>> Edges => _edges;

        public IReadOnlyList<INode<T>> Nodes => _nodes;

        public BiDirectionalGraph()
        {
            _nodes = new List<INode<T>>();
            _edges = new List<IEdge<T>>();
        }

        public INode<T> CreateNode(T value = default)
        {
            var node = new Node<T>(value);
            
            _nodes.Add(node);
            
            return node;
        }

        public void RemoveNode(INode<T> node)
        {
            if (!_nodes.Contains(node)) throw new ArgumentException();

            foreach (var edge in node.Edges.ToList()) Disconnect(edge);

            _nodes.Remove(node);
        }

        public IEdge<T> Connect(INode<T> first, INode<T> second)
        {
            if (!_nodes.Contains(first) || !_nodes.Contains(second)) throw new ArgumentException();

            var edge = new Edge<T>(first, second);
            (first as ICollection<IEdge<T>>).Add(edge);
            (second as ICollection<IEdge<T>>).Add(edge);
            _edges.Add(edge);

            return edge;
        }

        public void Disconnect(IEdge<T> edge)
        {
            if (!_edges.Contains(edge) || !_nodes.Contains(edge.From) || !_nodes.Contains(edge.To))
                throw new ArgumentException();

            (edge.From as ICollection<IEdge<T>>).Remove(edge);
            (edge.To as ICollection<IEdge<T>>).Remove(edge);
            _edges.Remove(edge);
        }
    }
}