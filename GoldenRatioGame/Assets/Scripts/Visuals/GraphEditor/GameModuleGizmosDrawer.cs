using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.Modules;
using UnityEngine;

namespace IM.ModuleGraphGizmosDebug
{
    public class GameModuleGizmosDrawer : MonoBehaviour
    {
        [SerializeField] private float _portSize = 1f;
        
        public IModuleGraphReadOnly Graph { get; set; }

        private void OnDrawGizmos()
        {
            if (Graph == null) return;

            ICoreGameModule start = Graph.Modules.FirstOrDefault(x => x is ICoreGameModule) as ICoreGameModule;
            if (start == null) return;

            Queue<(IGameModule, Vector3)> queue = new Queue<(IGameModule, Vector3)>();
            HashSet<IGameModule> visited = new HashSet<IGameModule>();
            queue.Enqueue((start, Vector3.zero));
            visited.Add(start);

            while (queue.Count > 0)
            {
                (IGameModule current, Vector3 position) = queue.Dequeue();

                Gizmos.color = Color.white;
                Gizmos.DrawWireCube(position, Vector3.one * 0.9f);

                foreach (IPortLayout portLayout in current.ModuleLayout.PortLayouts)
                {
                    IPort port = portLayout.Port;
                    Vector3 portPos = position + portLayout.RelativePosition;
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawSphere(portPos, 0.1f);

                    if (!port.IsConnected || port.Connection == null) continue;

                    IPort otherPort = GetOtherPort(port);
                    if (otherPort == null) continue;

                    if (otherPort.Module is not IGameModule nextModule || visited.Contains(nextModule)) continue;

                    IPortLayout nextLayout = nextModule.ModuleLayout.PortLayouts.FirstOrDefault(p => p.Port == otherPort);
                    if (nextLayout == null) continue;

                    Vector3 direction = portLayout.Normal.normalized;
                    Vector3 nextPos = position + direction;

                    nextPos += (portLayout.RelativePosition - nextLayout.RelativePosition);

                    Gizmos.color = Color.cyan;
                    
                    Gizmos.DrawLine(portPos, nextPos + nextLayout.RelativePosition);

                    visited.Add(nextModule);
                    queue.Enqueue((nextModule, nextPos));
                }
            }
        }

        private static IPort GetOtherPort(IPort port)
        {
            IConnection connection = port.Connection;
            if (connection == null) return null;
            return connection.Output == port ? connection.Input : connection.Output;
        }
    }
}