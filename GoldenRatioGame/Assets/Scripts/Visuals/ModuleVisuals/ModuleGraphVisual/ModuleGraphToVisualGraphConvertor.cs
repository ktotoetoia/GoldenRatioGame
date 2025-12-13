using System.Collections.Generic;
using System.Linq;
using IM.Base;
using IM.Graphs;
using IM.Modules;
using UnityEngine;

namespace IM.Visuals
{
    public class ModuleGraphToVisualGraphConvertor : IFactory<IVisualModuleGraph, IModuleGraphReadOnly>
    {
        private readonly ITraversal _traversal = new BreadthFirstTraversal();
        
        public Vector3 Position { get; set; }

        public IVisualModuleGraph Create(IModuleGraphReadOnly source)
        {
            Dictionary<IPort, IVisualPort> visualPortMap = new();
            Dictionary<IGameModule, IVisualModule> moduleToVisual = new Dictionary<IGameModule, IVisualModule>();
            ICoreGameModule coreModule = source.Modules.FirstOrDefault(x => x is ICoreGameModule) as ICoreGameModule;
            IVisualModuleGraph visualGraph = new DisposableVisualCommandModuleGraph(new HierarchyTransform(Position));

            foreach (IGameModule module in _traversal.Enumerate<IGameModule>(coreModule))
            {
                IVisualModule visualModule = module.Extensions.GetExtension<IModuleLayout>().CreateVisualModule(visualPortMap);
                
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
    }
}