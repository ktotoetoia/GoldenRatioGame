using System;
using System.Collections.Generic;
using System.Linq;

namespace IM.Graphs
{
    public class BiDirectionalGraph : IGraph
    {
        private readonly List<IEdge> _edges;
        private readonly List<INode> _nodes;

        public IEnumerable<IEdge> Edges => _edges;
        public IEnumerable<INode> Nodes => _nodes;

        public BiDirectionalGraph()
        {
            _nodes = new List<INode>();
            _edges = new List<IEdge>();
        }

        public INode CreateNode()
        {
            var node = new Node();
            
            _nodes.Add(node);
            
            return node;
        }

        public void RemoveNode(INode node)
        {
            if (!_nodes.Contains(node)) throw new ArgumentException();

            foreach (var edge in node.Edges.ToList()) Disconnect(edge);

            _nodes.Remove(node);
        }

        public IEdge Connect(INode first, INode second)
        {
            if (!_nodes.Contains(first) || !_nodes.Contains(second)) throw new ArgumentException();

            var edge = new Edge(first, second);
            (first as ICollection<IEdge>).Add(edge);
            (second as ICollection<IEdge>).Add(edge);
            _edges.Add(edge);

            return edge;
        }

        public void Disconnect(IEdge edge)
        {
            if (!_edges.Contains(edge) || !_nodes.Contains(edge.Node1) || !_nodes.Contains(edge.Node2))
                throw new ArgumentException();

            (edge.Node1 as ICollection<IEdge>).Remove(edge);
            (edge.Node2 as ICollection<IEdge>).Remove(edge);
            _edges.Remove(edge);
        }
    }
}