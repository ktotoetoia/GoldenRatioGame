using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.Modules;
using UnityEngine;

namespace IM.ModuleGraphGizmosDebug
{
    public class DecoratedGraphBuilder
    {
        private readonly ITraversal _traversal = new BreadthFirstTraversal();

        public Dictionary<IGameModule, DecoratedModule> Build(ICoreGameModule core)
        {
            Dictionary<IGameModule, DecoratedModule> map = new Dictionary<IGameModule, DecoratedModule>
            {
                [core] = new (core, Vector3.zero, Quaternion.identity)
            };

            foreach ((IGameModule node, IConnection incoming) in _traversal.EnumerateEdges<IGameModule, IConnection>(core))
            {
                if (incoming == null)
                {
                    if (!map.ContainsKey(node)) map[node] = new DecoratedModule(node, Vector3.zero, Quaternion.identity);
                    continue;
                }

                IGameModule toModule = node;
                IPortLayout toLayout = toModule.ModuleLayout.PortLayouts.FirstOrDefault(p => p.Port == incoming.Input);
                IPort fromPort = incoming.Output;
                if (fromPort.Module is not IGameModule fromModule || toLayout == null) continue;

                if (!map.TryGetValue(fromModule, out DecoratedModule fromDecorated)) continue;

                IPortLayout fromLayout = fromModule.ModuleLayout.PortLayouts.FirstOrDefault(p => p.Port == fromPort);
                if (fromLayout == null) continue;
                Vector3 fromPortWorldPos = fromDecorated.GetPortWorldPosition(fromLayout);
                Vector3 fromPortWorldNormal = fromDecorated.GetPortWorldNormal(fromLayout);

                Vector3 desiredToWorldNormal = -fromPortWorldNormal;

                Vector3 toLocalNormal = toLayout.Normal.normalized;
                Quaternion toRotation = Quaternion.FromToRotation(toLocalNormal, desiredToWorldNormal);

                Vector3 toWorldPos = fromPortWorldPos - (toRotation * toLayout.RelativePosition);

                if (!map.ContainsKey(toModule))
                {
                    map[toModule] = new DecoratedModule(toModule, toWorldPos, toRotation);
                }
            }

            return map;
        }
    }
}