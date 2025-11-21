using System.Collections.Generic;
using System.Linq;
using IM.Base;
using IM.Graphs;
using IM.Visuals;
using UnityEngine;
using Transform = IM.Visuals.Transform;

namespace IM.Modules
{
    public class ModuleGraphToVisualGraphConvertor : IFactory<IVisualModuleGraph, IModuleGraphReadOnly>
    {
        private readonly ITraversal _traversal = new BreadthFirstTraversal();
        
        public Vector3 Position {get; set; }
        
        public IVisualModuleGraph Create(IModuleGraphReadOnly source)
        {
            Dictionary<IPort, IVisualPort> visualPortMap = new();
            ICoreGameModule coreModule = source.Modules.FirstOrDefault(x => x is ICoreGameModule) as ICoreGameModule;
            IVisualModuleGraph visualGraph = new VisualCommandModuleGraph(new Transform(Position));
            Dictionary<IGameModule, IVisualModule> moduleToVisual = new Dictionary<IGameModule, IVisualModule>();

            foreach (IGameModule module in _traversal.Enumerate<IGameModule>(coreModule))
            {
                IVisualModule visualModule = Create(module.ModuleLayout,visualPortMap);
                moduleToVisual[module] = visualModule;
                visualGraph.AddModule(visualModule);
            }

            foreach (IConnection connection in source.Connections)
            {
                if (connection.Input?.Module is not IGameModule inputModule ||
                    connection.Output?.Module is not IGameModule outputModule ||
                    !moduleToVisual.TryGetValue(inputModule, out IVisualModule inputVisual) ||
                    !moduleToVisual.TryGetValue(outputModule, out IVisualModule outputVisual) ||
                    !visualPortMap.TryGetValue(connection.Input, out IVisualPort inputPort) ||
                    !visualPortMap.TryGetValue(connection.Output, out IVisualPort outputPort)) continue;
                
                visualGraph.Connect(outputPort, inputPort);
            }
            
            return visualGraph;
        }

        private IVisualModule Create(IModuleLayout moduleLayout ,Dictionary<IPort, IVisualPort> visualPortMap)
        {
            VisualModule visualModule = new (moduleLayout.Sprite)
            {
                Transform =
                {
                    Scale = moduleLayout.Bounds.size,
                }
            };

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