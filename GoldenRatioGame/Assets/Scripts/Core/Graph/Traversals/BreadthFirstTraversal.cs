using System;
using System.Collections.Generic;
using System.Linq;

namespace IM.Graphs
{
    public class BreadthFirstTraversal : ITraversal
    {
        public IGraphReadOnly GetSubGraph(INode start)
        {
            return GetSubGraph(start, x => true);
        }
        
        public IGraphReadOnly GetSubGraph(INode start, Func<IReadOnlyList<INode>, bool> canPathTo)
        {
            HashSet<INode> newNodes = new HashSet<INode>();
            HashSet<IEdge> newEdges = new HashSet<IEdge>();

            Queue<List<INode>> queue = new Queue<List<INode>>();
            List<INode> startPath = new List<INode> { start };

            newNodes.Add(start);
            queue.Enqueue(startPath);

            while (queue.Count > 0)
            {
                List<INode> path = queue.Dequeue();
                INode current = path[^1];

                foreach (IEdge edge in current.Edges)
                {
                    INode neighbor = edge.GetOther(current);

                    if (newNodes.Contains(neighbor))
                    {
                        if (newNodes.Contains(current))
                            newEdges.Add(edge);

                        continue;
                    }

                    List<INode> newPath = new List<INode>(path) { neighbor };

                    if (canPathTo(newPath))
                    {
                        newNodes.Add(neighbor);
                        newEdges.Add(edge);
                        queue.Enqueue(newPath);
                    }
                }
            }

            return new GraphReadOnly(newNodes.ToList(), newEdges.ToList());
        }

        public bool HasPathTo(INode from, INode to)
        {
            return GetSubGraph(from).Nodes.Contains(to);
        }
    }
} 