using System;

namespace IM.Graphs
{
    public class Edge : IEdge
    {
        public INode From { get; }
        public INode To { get; }


        public Edge(INode from, INode to)
        {
            From = from;
            To = to;
        }

        public INode GetOther(INode node)
        {
            if (node == null || (node != From && node != To))
                throw new ArgumentException();

            return node == From ? To : From;
        }
    }
}