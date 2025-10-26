using System.Collections.Generic;
using System.Linq;

namespace IM.Graphs
{
    public class BreadthFirstTraversal : ITraversal
    {
        public IEnumerable<TNode> Enumerate<TNode>(TNode start) where TNode : INode
        {
            Queue<TNode> queue = new Queue<TNode>();
            HashSet<TNode> visited = new HashSet<TNode>();

            queue.Enqueue(start);
            visited.Add(start);

            while (queue.Count > 0)
            {
                TNode current = queue.Dequeue();
                yield return current;

                foreach (IEdge edge in current.Edges)
                {
                    if (edge.GetOther(current) is TNode next && visited.Add(next))
                        queue.Enqueue(next);
                }
            }
        }

        public IEnumerable<(TNode, TEdge)> EnumerateEdges<TNode, TEdge>(TNode start)
            where TNode : INode
            where TEdge : IEdge
        {
            Queue<(TNode, TEdge)> queue = new Queue<(TNode, TEdge)>();
            HashSet<TNode> visited = new HashSet<TNode>();

            queue.Enqueue((start, default));
            visited.Add(start);

            while (queue.Count > 0)
            {
                (TNode current, TEdge via) = queue.Dequeue();
                yield return (current, via);

                foreach (IEdge edge in current.Edges)
                {
                    if (edge.GetOther(current) is TNode next && visited.Add(next))
                    {
                        if (edge is TEdge typedEdge)
                            queue.Enqueue((next, typedEdge));
                    }
                }
            }
        }

        public IEnumerable<INode> Enumerate(INode start)
            => Enumerate<INode>(start);
        public IEnumerable<(INode, IEdge)> EnumerateEdges(INode start)
            => EnumerateEdges<INode, IEdge>(start);
        
        public bool HasPathTo(INode from, INode to)
        {
            foreach (INode node in Enumerate(from))
                if (Equals(node, to))
                    return true;
            return false;
        }
    }
} 