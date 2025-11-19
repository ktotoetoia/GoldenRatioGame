using System.Collections.Generic;
using System.Linq;
using IM.Base;
using IM.Graphs;
using IM.Visuals;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleGraphToVisualGraphConvertor : IFactory<IVisualModuleGraph, IModuleGraphReadOnly>
    {
        public IVisualModuleGraph Create(IModuleGraphReadOnly source)
        {
            Dictionary<IPort, IVisualPort> visualPortMap = new();
            ICoreGameModule coreModule = source.Modules.FirstOrDefault(x => x is ICoreGameModule) as ICoreGameModule;
            IVisualModuleGraph visualGraph = new VisualCommandModuleGraph();
            
            BreadthFirstTraversal traversal = new BreadthFirstTraversal();
            Dictionary<IGameModule, IVisualModule> moduleToVisual = new Dictionary<IGameModule, IVisualModule>();

            foreach (IGameModule module in traversal.Enumerate<IGameModule>(coreModule))
            {
                IVisualModule visualModule = Create(module.ModuleLayout,visualPortMap,visualGraph.Transform);
                moduleToVisual[module] = visualModule;
                visualGraph.AddModule(visualModule);
            }

            foreach (IConnection connection in source.Connections)
            {
                if (connection.Input?.Module is not IGameModule inputModule ||
                    connection.Output?.Module is not IGameModule outputModule)
                    continue;

                if (!moduleToVisual.TryGetValue(inputModule, out IVisualModule inputVisual) ||
                    !moduleToVisual.TryGetValue(outputModule, out IVisualModule outputVisual))
                    continue;

                if (!visualPortMap.TryGetValue(connection.Input, out IVisualPort inputPort) ||
                    !visualPortMap.TryGetValue(connection.Output, out IVisualPort outputPort))
                    continue;

                visualGraph.Connect(outputPort, inputPort);
            }
            
            return visualGraph;
        }

        private IVisualModule Create(IModuleLayout moduleLayout ,Dictionary<IPort, IVisualPort> visualPortMap, ITransform t)
        {
            VisualModule visualModule = new VisualModule(moduleLayout.Sprite);
            t.AddChild(visualModule.Transform);

            visualModule.Transform.Scale = moduleLayout.Bounds.size;
            
            foreach (IPortLayout portLayout in moduleLayout.PortLayouts)
            {
                IVisualPort visualPort = new VisualPort(visualModule);
                
                visualModule.Transform.AddChild(visualPort.Transform);

                visualPort.Transform.LocalPosition = portLayout.RelativePosition;
                visualPort.Transform.LocalScale = Vector3.one;
                visualPort.Transform.LocalRotation = Quaternion.LookRotation(portLayout.Normal, Vector3.up);
                
                visualModule.AddPort(visualPort);
                visualPortMap[portLayout.Port] = visualPort;
            }

            return visualModule;
        }
    }
}