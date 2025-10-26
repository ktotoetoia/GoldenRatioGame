using System.Collections.Generic;

namespace IM.Graphs
{
    public interface ITraversal
    {
        IEnumerable<INode> Enumerate(INode start);
        IEnumerable<TNode> Enumerate<TNode>(TNode start) where TNode : INode;
        IEnumerable<(INode, IEdge)> EnumerateEdges(INode start);
        IEnumerable<(TNode, TEdge)> EnumerateEdges<TNode,TEdge>(TNode start)  where TNode : INode where TEdge : IEdge;
        
        bool HasPathTo(INode from, INode to);
    }
} 