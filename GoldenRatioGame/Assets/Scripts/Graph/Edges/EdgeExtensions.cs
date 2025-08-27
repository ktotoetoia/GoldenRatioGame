using System;

namespace IM.Graphs
{
    public static class EdgeExtensions
    {
        public static INode<T> GetOther<T>(this IEdge<T> edge, INode<T> node)
        {
            if(edge.From == node) return edge.To;
            if(edge.To == node) return edge.From;

            throw new SystemException("edge does not contains this node");
        }
    }
}