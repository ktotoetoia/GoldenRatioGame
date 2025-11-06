using System;
using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.ModuleGraph;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleGraphVisual : MonoBehaviour, IModuleGraphVisual
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
            VisualModule visualModule = new VisualModule();

            foreach (IPortLayout portLayout in moduleLayout.PortLayouts)
            {
                IVisualPort visualPort = new VisualPort(
                    visualModule,
                    portLayout.RelativePosition,
                    portLayout.Normal
                );

                visualModule.AddPort(visualPort);
                _visualPortMap[portLayout.Port] = visualPort;
            }

            return visualModule;
        }

        private void OnDrawGizmos()
        {
            if (_visualGraph == null) return;

            Gizmos.color = Color.white;
            foreach (IVisualModule module in _visualGraph.Modules)
            {
                Gizmos.DrawWireCube(module.Position, Vector3.one);

                Gizmos.color = Color.yellow;
                foreach (IVisualPort port in module.Ports)
                {
                    Vector3 portPos = module.Position + port.RelativePosition;
                    Gizmos.DrawSphere(portPos, 0.1f);
                }
            }

            Gizmos.color = Color.cyan;
            foreach (IVisualConnection connection in _visualGraph.Connections)
            {
                Debug.Log(connection);
                
                Vector3 from = connection.Output.Position;
                Vector3 to = connection.Input.Position;
                Gizmos.DrawLine(from, to);
            }
        }
    }
}