using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace IM.Modules
{
    [ExecuteAlways]
    public class ModuleTreeGizmosDrawer : MonoBehaviour
    {
        public IModule2 RootModuleBehaviour { get; set; }

        [Header("Layout")]
        public float LevelSpacing = 2.0f;
        public float SiblingSpacing = 2.0f;
        public Transform Origin;
        public bool CenterEachLevel = true;

        [Header("Visuals")]
        public float NodeRadius = 0.15f;
        public Color NodeColor = Color.cyan;
        public Color RootColor = Color.yellow;
        public Color EdgeColor = Color.white;

        public bool DrawLabels = true;

        private void OnDrawGizmos()
        {
            if (RootModuleBehaviour == null) return;
            if (RootModuleBehaviour is not IModule2 root) return;

            Vector3 origin = Origin ? Origin.position : transform.position;

            var (levels, perLevel) = BuildLevels(root);

            var positions = ComputeLayoutPositions(perLevel, origin, SiblingSpacing, LevelSpacing, CenterEachLevel);

            // Draw edges (connectors)
            Gizmos.color = EdgeColor;
            foreach (var kv in positions)
            {
                var module = kv.Key;
                var pos = kv.Value;

                foreach (var connector in module.Connectors)
                {
                    if (connector.To == null || !positions.ContainsKey(connector.To)) continue;
                    Gizmos.DrawLine(pos, positions[connector.To]);
                }
            }

            // Draw nodes
            foreach (var kv in positions)
            {
                var module = kv.Key;
                var pos = kv.Value;

                Gizmos.color = (module == root) ? RootColor : NodeColor;
                Gizmos.DrawSphere(pos, NodeRadius);

#if UNITY_EDITOR
                if (DrawLabels && module is Object unityObj)
                {
                    Handles.color = Color.white;
                    Handles.Label(pos + Vector3.up * (NodeRadius * 1.5f), unityObj.name);
                }
#endif
            }
        }

        private static (Dictionary<IModule2, int> levels, Dictionary<int, List<IModule2>> perLevel)
            BuildLevels(IModule2 root)
        {
            var levels = new Dictionary<IModule2, int>();
            var perLevel = new Dictionary<int, List<IModule2>>();
            var q = new Queue<IModule2>();
            var visited = new HashSet<IModule2>();

            q.Enqueue(root);
            visited.Add(root);
            levels[root] = 0;
            perLevel[0] = new List<IModule2> { root };

            while (q.Count > 0)
            {
                var node = q.Dequeue();
                if (node.Connectors == null) continue;

                foreach (var c in node.Connectors)
                {
                    var child = c.To;
                    if (child == null || visited.Contains(child)) continue;

                    visited.Add(child);
                    int lvl = levels[node] + 1;
                    levels[child] = lvl;

                    if (!perLevel.TryGetValue(lvl, out var list))
                    {
                        list = new List<IModule2>();
                        perLevel[lvl] = list;
                    }
                    list.Add(child);

                    q.Enqueue(child);
                }
            }

            return (levels, perLevel);
        }

        private static Dictionary<IModule2, Vector3> ComputeLayoutPositions(
            Dictionary<int, List<IModule2>> perLevel,
            Vector3 origin,
            float siblingSpacing,
            float levelSpacing,
            bool centerEachLevel)
        {
            var positions = new Dictionary<IModule2, Vector3>();
            foreach (var kv in perLevel)
            {
                int level = kv.Key;
                var list = kv.Value;
                int count = list.Count;

                float startX = origin.x;
                if (centerEachLevel && count > 1)
                {
                    float width = (count - 1) * siblingSpacing;
                    startX -= width * 0.5f;
                }

                for (int i = 0; i < count; i++)
                {
                    float x = (centerEachLevel ? startX + i * siblingSpacing
                        : origin.x + i * siblingSpacing);
                    float y = origin.y - level * levelSpacing;
                    var pos = new Vector3(x, y, origin.z);
                    positions[list[i]] = pos;
                }
            }
            return positions;
        }
    }
}