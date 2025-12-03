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
        private readonly IFactory<IVisualModule, IModuleLayout, IDictionary<IPort, IVisualPort>> _moduleConverter;
        private readonly ITraversal _traversal = new BreadthFirstTraversal();
        
        public Vector3 Position { get; set; }

        public ModuleGraphToVisualGraphConvertor() : this(new ModuleLayoutToVisualModuleConvertor())
        {
            
        }

        public ModuleGraphToVisualGraphConvertor(
            IFactory<IVisualModule, IModuleLayout, IDictionary<IPort, IVisualPort>> moduleConverter)
        {
            _moduleConverter = moduleConverter;
        }
        
        public IVisualModuleGraph Create(IModuleGraphReadOnly source)
        {
            Dictionary<IPort, IVisualPort> visualPortMap = new();
            ICoreGameModule coreModule = source.Modules.FirstOrDefault(x => x is ICoreGameModule) as ICoreGameModule;
            IVisualModuleGraph visualGraph = new VisualCommandModuleGraph(new Transform(Position));
            Dictionary<IGameModule, IVisualModule> moduleToVisual = new Dictionary<IGameModule, IVisualModule>();

            foreach (IGameModule module in _traversal.Enumerate<IGameModule>(coreModule))
            {
                IVisualModule visualModule = _moduleConverter.Create(module.Extensions.GetExtension<IModuleLayout>(), visualPortMap);
                
                moduleToVisual[module] = visualModule;
                Debug.Log("adding");
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