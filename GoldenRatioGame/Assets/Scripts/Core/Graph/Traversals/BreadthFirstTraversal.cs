using System.Collections.Generic;

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
                    if (edge.GetOther(current) is TNode next && visited.Add(next) && edge is TEdge typedEdge)
                    {
                        queue.Enqueue((next, typedEdge));
                    }
                }
            }
        }

        public IEnumerable<(TModule, TPort)> EnumerateModules<TModule, TPort>(TModule start) 
            where TModule : IModule 
            where TPort : IPort
        {
            Queue<(TModule, TPort)> queue = new Queue<(TModule, TPort)>();
            HashSet<TModule> visited = new HashSet<TModule>();

            queue.Enqueue((start, default));
            visited.Add(start);

            while (queue.Count > 0)
            {
                (TModule current, TPort via) = queue.Dequeue();
                yield return (current, via);

                foreach (IPort port in current.Ports)
                {
                    if (port.IsConnected && port.Connection.GetOtherPort(port).Module is TModule next && visited.Add(next) && port is TPort typedPort)
                    {
                        queue.Enqueue((next, typedPort));
                    }
                }
            }
        }

        public bool HasPathTo(INode from, INode to)
        {
            foreach (INode node in Enumerate(from))
                if (Equals(node, to))
                    return true;
            return false;
        }
    }
} 