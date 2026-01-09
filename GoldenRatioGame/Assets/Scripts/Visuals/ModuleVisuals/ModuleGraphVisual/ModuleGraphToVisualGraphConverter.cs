using System.Collections.Generic;
using System.Linq;
using IM.Base;
using IM.Graphs;
using IM.Modules;
using UnityEngine;

namespace IM.Visuals
{
    public class ModuleGraphToVisualGraphConverter : IFactory<ITransformModuleGraph, IModuleGraphReadOnly>
    {
        private readonly ITraversal _traversal = new BreadthFirstTraversal();
        
        public Vector3 Position { get; set; }

        public ITransformModuleGraph Create(IModuleGraphReadOnly source)
        {
            Dictionary<IPort, ITransformPort> visualPortMap = new();
            Dictionary<IGameModule, ITransformModule> moduleToVisual = new Dictionary<IGameModule, ITransformModule>();
            ICoreGameModule coreModule = source.Modules.FirstOrDefault(x => x is ICoreGameModule) as ICoreGameModule;
            ITransformModuleGraph transformGraph = new TransformCommandModuleGraph();
            
            foreach (IGameModule module in _traversal.Enumerate<IGameModule>(coreModule))
            {
                IModuleAnimationController moduleAnimationController = module.Extensions.GetExtension<IModuleAnimationController>();
                ITransformModule transformModule = moduleAnimationController.CreateNewReferenceModule();

                foreach (IPort port in module.Ports)
                {
                    visualPortMap[port] = moduleAnimationController.GetReferencePort(port);
                }
                
                moduleToVisual[module] = transformModule;
                transformGraph.AddModule(transformModule);
            }

            foreach (IConnection connection in source.Connections)
            {
                if (connection.Port1?.Module is not IGameModule inputModule ||
                    connection.Port2?.Module is not IGameModule outputModule ||
                    !moduleToVisual.TryGetValue(inputModule, out ITransformModule inputVisual) ||
                    !moduleToVisual.TryGetValue(outputModule, out ITransformModule outputVisual) ||
                    !visualPortMap.TryGetValue(connection.Port1, out ITransformPort inputPort) ||
                    !visualPortMap.TryGetValue(connection.Port2, out ITransformPort outputPort)) continue;
                
                transformGraph.Connect(outputPort, inputPort);
            }
            
            transformGraph.Transform.Position = Position;
            return transformGraph;
        }
    }
}