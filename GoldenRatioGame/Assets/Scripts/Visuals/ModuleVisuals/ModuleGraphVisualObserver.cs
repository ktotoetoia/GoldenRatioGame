using System;
using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.ModuleGraph;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleGraphVisualObserver : MonoBehaviour, IModuleGraphObserver
    {
        private readonly Dictionary<IPort, IVisualPort> _visualPortMap = new();
        
        public void OnGraphUpdated(IModuleGraphReadOnly graph)
        {
            if (graph == null) throw new ArgumentNullException(nameof(graph));
            
            GetComponent<IModuleGraphVisual>().Source = CreateVisualGraph(graph);
        }
        
        private IVisualModuleGraph CreateVisualGraph(IModuleGraphReadOnly source)
        {
            ICoreGameModule coreModule = source.Modules.FirstOrDefault(x => x is ICoreGameModule) as ICoreGameModule;
            IVisualModuleGraph visualGraph = new VisualCommandModuleGraph();
            
            BreadthFirstTraversal traversal = new BreadthFirstTraversal();
            Dictionary<IGameModule, IVisualModule> moduleToVisual = new Dictionary<IGameModule, IVisualModule>();

            foreach (IGameModule module in traversal.Enumerate<IGameModule>(coreModule))
            {
                IVisualModule visualModule = Create(module.ModuleLayout);
                moduleToVisual[module] = visualModule;
                visualGraph.AddModule(visualModule);
                visualModule.Sprite =  module.ModuleLayout.Sprite;
            }

            foreach (IConnection connection in source.Connections)
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

                visualGraph.Connect(outputPort, inputPort);
            }
            
            return visualGraph;
        }

        private IVisualModule Create(IModuleLayout moduleLayout)
        {
            GizmosVisualModule gizmosVisualModule = new GizmosVisualModule(new ModuleGraph.Transform(Vector3.zero, Vector3.one, new Quaternion(0,0,0,1)));
            
            foreach (IPortLayout portLayout in moduleLayout.PortLayouts)
            {
                IVisualPort visualPort = new VisualPort(
                    gizmosVisualModule,
                    new ModuleGraph.Transform(gizmosVisualModule.Transform, portLayout.RelativePosition,Vector3.one,  Quaternion.LookRotation(portLayout.Normal, Vector3.up))
                );

                gizmosVisualModule.AddPort(visualPort);
                _visualPortMap[portLayout.Port] = visualPort;
            }

            return gizmosVisualModule;
        }
    }
}