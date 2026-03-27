using System;
using System.Collections.Generic;
using System.Linq;

namespace IM.Graphs
{
    public class BiDirectionalDataGraph<T> : IDataGraph<T>
    {
        private readonly List<IDataNode<T>> _nodes = new();
        private readonly List<IDataEdge<T>> _edges= new();

        public IEnumerable<INode> Nodes => _nodes;
        public IEnumerable<IEdge> Edges =>  _edges;
        public IEnumerable<IDataNode<T>> DataNodes => _nodes;
        public IEnumerable<IDataEdge<T>> DataEdges => _edges;
        
        public IDataNode<T> Create(T data)
        {
            DataNode<T> node = new DataNode<T>(data);
            
            _nodes.Add(node);

            return node;
        }

        public IDataEdge<T> Connect(IDataNode<T> dataNode1, IDataNode<T> dataNode2)
        {
            if (!_nodes.Contains(dataNode1) || !_nodes.Contains(dataNode2) || dataNode1 == dataNode2) throw new ArgumentException();

            DataEdge<T> edge = new DataEdge<T>(dataNode1, dataNode2);
            
            (dataNode1 as ICollection<IDataEdge<T>>).Add(edge);
            (dataNode2 as ICollection<IDataEdge<T>>).Add(edge);

            _edges.Add(edge);

            return edge;
        }

        public void Disconnect(IDataEdge<T> edge)
        {
            if (!_edges.Contains(edge) || !_nodes.Contains(edge.DataNode1) || !_nodes.Contains(edge.DataNode2))
                throw new ArgumentException();

            (edge.Node1 as ICollection<IDataEdge<T>>).Remove(edge);
            (edge.Node2 as ICollection<IDataEdge<T>>).Remove(edge);
            
            _edges.Remove(edge);
        }

        public IEnumerable<IDataNode<T>> RemoveAll(T data)
        {
            List<IDataNode<T>> toRemove = _nodes.Where(x => x.Value.Equals(data)).ToList();
            
            foreach (IDataNode<T> node in toRemove) Remove(node);
            
            return toRemove;
        }

        public void Remove(IDataNode<T> node)
        {
            if (!_nodes.Contains(node)) throw new ArgumentException();

            foreach (var edge in node.DataEdges.ToList()) Disconnect(edge);

            _nodes.Remove(node);
        }
    }
}