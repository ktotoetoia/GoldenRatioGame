using System;

namespace IM.Graphs
{
    public class Edge<T> : IEdge<T>, IValue<T>
    {
        public T Value { get; set; }
        public INode<T> From { get; }
        public INode<T> To { get; }

        public Edge(INode<T> from, INode<T> to) : this(from, to, default) { }

        public Edge(INode<T> from, INode<T> to, T value)
        {
            From = from;
            To = to;
            Value = value;
        }

        public INode<T> GetOther(INode<T> node)
        {
            if (node == null || (node != From && node != To))
                throw new ArgumentException();

            return node == From ? To : From;
        }
    }
}