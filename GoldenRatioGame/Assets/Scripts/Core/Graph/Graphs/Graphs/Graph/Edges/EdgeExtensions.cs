using System;

namespace IM.Graphs
{
    public static class EdgeExtensions
    {
        public static INode GetOther(this IEdge edge, INode node)
        {
            if(edge.Node1 == node) return edge.Node2;
            if(edge.Node2 == node) return edge.Node1;

            throw new SystemException("edge does not contains this node");
        }
    }
}