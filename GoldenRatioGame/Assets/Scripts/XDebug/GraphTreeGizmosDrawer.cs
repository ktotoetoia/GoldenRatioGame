using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using UnityEditor;
using UnityEngine;

namespace IM.Modules
{
    [ExecuteAlways]
    public class GraphTreeGizmosDrawer : MonoBehaviour
    {
        private IModuleEntity _entity;
        public IGraphReadOnly Graph { get; set; }

        [SerializeField] private bool _isOn = true;

        [Header("Layout")]
        [SerializeField] private float _levelSpacing    = 2.0f;
        [SerializeField] private float _siblingSpacing  = 2.0f;
        [SerializeField] private Transform _origin;
        [SerializeField] private bool _centerEachLevel = true;

        [Header("Visuals")]
        [SerializeField] private float _nodeRadius = 0.15f;
        [SerializeField] private Color _modeColor  = Color.cyan;
        [SerializeField] private Color _rootColor  = Color.yellow;
        [SerializeField] private Color _edgeColor  = Color.white;
        
        [SerializeField] private bool _drawLabels = true;

        private void Awake()
        {
            _entity = _origin.GetComponent<IModuleEntity>();
        }

        private void OnDrawGizmos()
        {
            if(!_isOn) return;
            
            if (_entity is { GraphEditor: not null })
            {
                Graph = _entity.GraphEditor.Graph;
            }
            
            if (Graph == null || Graph.Nodes == null || Graph.Edges == null) return;

            Vector3 origin = _origin ? _origin.position : transform.position;

            var roots = FindRootNodes(Graph);
            if (roots.Count == 0) return;

            var (levels, perLevel) = BuildLevels(Graph, roots);
            var positions = ComputeLayout(perLevel, origin, _siblingSpacing, _levelSpacing, _centerEachLevel);

            Gizmos.color = _edgeColor;
            foreach (var edge in Graph.Edges)
            {
                if (positions.TryGetValue(edge.From, out var p1) &&
                    positions.TryGetValue(edge.To,   out var p2))
                {
                    Gizmos.DrawLine(p1, p2);
                }
            }

            foreach (var kv in positions)
            {
                var node = kv.Key;
                var pos  = kv.Value;

                Gizmos.color = roots.Contains(node) ? _rootColor : _modeColor;
                Gizmos.DrawSphere(pos, _nodeRadius);

#if UNITY_EDITOR
                if (_drawLabels && node is Object uo)
                {
                    Handles.color = Color.white;
                    Handles.Label(pos + Vector3.up * (_nodeRadius * 1.5f), uo.name);
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