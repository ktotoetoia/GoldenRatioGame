using System;
using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace IM.Modules
{
    [ExecuteAlways]
    public class GraphTreeGizmosDrawer : MonoBehaviour
    {
        public IGraphReadOnly Graph { get; set; }

        [Header("Layout")]
        public float LevelSpacing    = 2.0f;
        public float SiblingSpacing  = 2.0f;
        public Transform Origin;
        public bool CenterEachLevel = true;

        [Header("Visuals")]
        public float NodeRadius = 0.15f;
        public Color NodeColor  = Color.cyan;
        public Color RootColor  = Color.yellow;
        public Color EdgeColor  = Color.white;

        public bool DrawLabels = true;

        private void OnDrawGizmos()
        {
            if (Graph == null || Graph.Nodes == null || Graph.Edges == null) return;

            Vector3 origin = Origin ? Origin.position : transform.position;

            var roots = FindRootNodes(Graph);
            if (roots.Count == 0) return;

            var (levels, perLevel) = BuildLevels(Graph, roots);
            var positions = ComputeLayout(perLevel, origin, SiblingSpacing, LevelSpacing, CenterEachLevel);

            // Draw edges
            Gizmos.color = EdgeColor;
            foreach (var edge in Graph.Edges)
            {
                if (positions.TryGetValue(edge.From, out var p1) &&
                    positions.TryGetValue(edge.To,   out var p2))
                {
                    Gizmos.DrawLine(p1, p2);
                }
            }

            // Draw nodes
            foreach (var kv in positions)
            {
                var node = kv.Key;
                var pos  = kv.Value;

                Gizmos.color = roots.Contains(node) ? RootColor : NodeColor;
                Gizmos.DrawSphere(pos, NodeRadius);

#if UNITY_EDITOR
                if (DrawLabels && node is Object uo)
                {
                    Handles.color = Color.white;
                    Handles.Label(pos + Vector3.up * (NodeRadius * 1.5f), uo.name);
                }
#endif
            }
        }

        private static List<INode> FindRootNodes(IGraphReadOnly graph)
        {
            var allNodes = new HashSet<INode>(graph.Nodes);
            var targets  = new HashSet<INode>(graph.Edges.Select(e => e.To));
            allNodes.ExceptWith(targets);
            return allNodes.ToList();
        }

        private static (Dictionary<INode,int>, Dictionary<int,List<INode>>)
            BuildLevels(IGraphReadOnly graph, List<INode> roots)
        {
            var levels   = new Dictionary<INode,int>();
            var perLevel = new Dictionary<int,List<INode>>();
            var queue    = new Queue<INode>();
            var seen     = new HashSet<INode>();

            foreach (var root in roots)
            {
                queue.Enqueue(root);
                seen.Add(root);
                levels[root] = 0;
                if (!perLevel.ContainsKey(0))
                    perLevel[0] = new List<INode>();
                perLevel[0].Add(root);
            }

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                int curLevel = levels[current];

                foreach (var edge in graph.Edges.Where(e => e.From == current))
                {
                    var child = edge.To;
                    if (child == null || seen.Contains(child)) continue;

                    seen.Add(child);
                    int childLevel = curLevel + 1;
                    levels[child] = childLevel;

                    if (!perLevel.TryGetValue(childLevel, out var list))
                    {
                        list = new List<INode>();
                        perLevel[childLevel] = list;
                    }
                    list.Add(child);

                    queue.Enqueue(child);
                }
            }

            return (levels, perLevel);
        }

        private static Dictionary<INode, Vector3> ComputeLayout(
            Dictionary<int,List<INode>> perLevel,
            Vector3 origin,
            float siblingSpacing,
            float levelSpacing,
            bool centerEachLevel)
        {
            var result = new Dictionary<INode, Vector3>();

            foreach (var kv in perLevel)
            {
                int level = kv.Key;
                var nodes = kv.Value;
                int count = nodes.Count;

                float startX = origin.x;
                if (centerEachLevel && count > 1)
                {
                    float totalWidth = (count - 1) * siblingSpacing;
                    startX -= totalWidth * 0.5f;
                }

                for (int i = 0; i < count; i++)
                {
                    float x = centerEachLevel
                        ? startX + i * siblingSpacing
                        : origin.x   + i * siblingSpacing;

                    float y = origin.y - level * levelSpacing;
                    var pos = new Vector3(x, y, origin.z);

                    result[nodes[i]] = pos;
                }
            }

            return result;
        }
    }
}
