using System;
using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.ModuleGraph;
using IM.Values;
using UnityEngine;
using Transform = IM.ModuleGraph.Transform;

namespace IM.Modules
{
    public class ModuleGraphVisual : MonoBehaviour,IModuleGraphVisual
    {
        public IModuleGraphReadOnly Source { get; private set; }

        private IVisualCommandModuleGraph _visualGraph;

        private readonly Dictionary<IPort, IVisualPort> _visualPortMap = new();

        public void SetSource(IModuleGraphReadOnly source, ICoreGameModule coreModule)
        {
            Source = source;
            _visualGraph = new VisualCommandModuleGraph();
            _visualPortMap.Clear();
            
            var traversal = new BreadthFirstTraversal();
            var moduleToVisual = new Dictionary<IGameModule, IVisualModule>();

            foreach (IGameModule module in traversal.Enumerate<IGameModule>(coreModule))
            {
                IVisualModule visualModule = Create(module.ModuleLayout);
                moduleToVisual[module] = visualModule;
                _visualGraph.AddModule(visualModule);
            }

            foreach (IConnection connection in Source.Connections)
            {
                if (connection.Input?.Module is not IGameModule inputModule ||
                    connection.Output?.Module is not IGameModule outputModule)
                    continue;

                if (!moduleToVisual.TryGetValue(inputModule, out IVisualModule inputVisual) ||
                    !moduleToVisual.TryGetValue(outputModule, out IVisualModule outputVisual))
                    continue;

                if (!_visualPortMap.TryGetValue(connection.Input, out IVisualPort inputPort) ||
                    !_visualPortMap.TryGetValue(connection.Output, out IVisualPort outputPort))
                    continue;

                _visualGraph.Connect(outputPort, inputPort);
            }
        }

        private IVisualModule Create(IModuleLayout moduleLayout)
        {
            GizmosVisualModule gizmosVisualModule = new GizmosVisualModule(new Transform(Vector3.zero, Vector3.one, new Quaternion(0,0,0,1)));
            foreach (IPortLayout portLayout in moduleLayout.PortLayouts)
            {
                IVisualPort visualPort = new VisualPort(
                    gizmosVisualModule,
                    new Transform(gizmosVisualModule.Transform, portLayout.RelativePosition,Vector3.one,  Quaternion.LookRotation(portLayout.Normal, Vector3.up))
                );

                gizmosVisualModule.AddPort(visualPort);
                _visualPortMap[portLayout.Port] = visualPort;
            }

            return gizmosVisualModule;
        }

        private void OnDrawGizmos()
        {
            if (_visualGraph == null) return;

            foreach (IVisualModule module in _visualGraph.Modules)
            {
                IEnumerable<Vector3> worldPorts = module.Ports.Select(p => p.Transform.Position);
                IEnumerable<Vector3> localPorts = worldPorts.Select(w =>
                {
                    Vector3 local = Quaternion.Inverse(module.Transform.Rotation) * (w - module.Transform.Position);
                    return new Vector3(
                        local.x / module.Transform.Scale.x,
                        local.y / module.Transform.Scale.y,
                        local.z / module.Transform.Scale.z
                    );
                });

                Bounds localBounds = BoundsUtility.CreateBoundsNormalized(localPorts);

                Matrix4x4 oldMatrix = Gizmos.matrix;
                Gizmos.matrix = Matrix4x4.TRS(
                    module.Transform.Position,
                    module.Transform.Rotation,
                    Vector3.one);

                Gizmos.color = Color.white;
                Gizmos.DrawWireCube(Vector3.zero, localBounds.size);

                Gizmos.matrix = oldMatrix;

                foreach (IVisualPort port in module.Ports)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawSphere(port.Transform.Position, 0.1f);
                }
            }

            Gizmos.color = Color.cyan;
            foreach (IVisualConnection connection in _visualGraph.Connections)
            {
                Vector3 from = connection.Output.Transform.Position;
                Vector3 to = connection.Input.Transform.Position;
                Gizmos.DrawLine(from, to);
            }
        }
    }
}