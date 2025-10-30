using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.Modules;
using UnityEngine;

namespace IM.ModuleGraphGizmosDebug
{
    public class ModuleGraphGizmosDrawer : MonoBehaviour, IModuleGraphVisual
    {
        [SerializeField] private float _portSize = 1f;

        public IModuleGraphReadOnly Source { get; set; }
        
        public void RebuildSource()
        {
        }

        private void OnDrawGizmos()
        {
            ICoreGameModule start = Source?.Modules.OfType<ICoreGameModule>().FirstOrDefault();
            if (start == null) return;

            BreadthFirstTraversal traversal = new BreadthFirstTraversal();
            Dictionary<IGameModule, Vector3> positions = new Dictionary<IGameModule, Vector3> { [start] = Vector3.zero };

            foreach ((IGameModule node, IConnection edge) in traversal.EnumerateEdges<IGameModule, IConnection>(start))
            {
                if (!positions.ContainsKey(node))
                {
                    positions[node] = edge != null ? 
                        CalculateNextPosition(node, edge, positions) : 
                        Vector3.zero;
                }

                Vector3 pos = positions[node];

                Gizmos.color = Color.white;
                Gizmos.DrawWireCube(pos, Vector3.one * 0.9f);

                foreach (IPortLayout portLayout in node.ModuleLayout.PortLayouts)
                {
                    Vector3 portPos = pos + portLayout.RelativePosition;
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawSphere(portPos, 0.1f);

                    IPort otherPort = GetOtherPort(portLayout.Port);
                    if (otherPort?.Module is not IGameModule nextModule) continue;
                    if (!positions.TryGetValue(nextModule, out Vector3 nextPos)) continue;

                    IPortLayout nextLayout = nextModule.ModuleLayout.PortLayouts.FirstOrDefault(p => p.Port == otherPort);
                    if (nextLayout == null) continue;

                    Gizmos.color = Color.cyan;
                    Gizmos.DrawLine(portPos, nextPos + nextLayout.RelativePosition);
                }
            }
        }

        private Vector3 CalculateNextPosition(IGameModule node, IConnection edge, Dictionary<IGameModule, Vector3> positions)
        {
            IGameModule prevModule = positions.ContainsKey(edge.From as IGameModule) ? edge.From as IGameModule : edge.To as IGameModule;
            Vector3 prevPos = positions[prevModule];

            IPortLayout fromLayout = prevModule.ModuleLayout.PortLayouts.FirstOrDefault(p => p.Port == edge.Output);
            IPortLayout toLayout = node.ModuleLayout.PortLayouts.FirstOrDefault(p => p.Port == edge.Input);
            if (fromLayout == null || toLayout == null) return prevPos;

            return prevPos + fromLayout.Normal.normalized + (fromLayout.RelativePosition - toLayout.RelativePosition);
        }

        private IPort GetOtherPort(IPort port) => port.Connection?.Output == port ? port.Connection.Input : port.Connection?.Output;
    }
}