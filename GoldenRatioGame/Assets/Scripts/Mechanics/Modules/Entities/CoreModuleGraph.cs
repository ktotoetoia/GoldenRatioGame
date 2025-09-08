using System;
using System.Collections.Generic;
using IM.Graphs;

namespace IM.Modules
{
    public class CoreModuleGraph : ICoreModuleGraph
    {
        private readonly IModuleGraph _moduleGraph;
        public IReadOnlyList<INode> Nodes => _moduleGraph.Nodes;
        public IReadOnlyList<IEdge> Edges => _moduleGraph.Edges;
        public IEnumerable<IModuleConnection> Connections => _moduleGraph.Connections;
        public IEnumerable<IModule> Modules => _moduleGraph.Modules;
        public IModule CoreModule { get; private set; }

        public CoreModuleGraph(IModule coreModule) :this(coreModule,new SafeModuleGraph())
        {
            
        }
        
        public CoreModuleGraph(IModule coreModule, IModuleGraph moduleGraph)
        {
            _moduleGraph = moduleGraph;
            SetCoreModule(coreModule);
        }
        
        public void AddModule(IModule module)
        {
            _moduleGraph.AddModule(module);
        }

        public void RemoveModule(IModule module)
        {
            _moduleGraph.RemoveModule(module);
        }

        public IModuleConnection Connect(IModulePort output, IModulePort input)
        {
            return _moduleGraph.Connect(output, input);
        }

        public void Disconnect(IModuleConnection connection)
        {
            _moduleGraph.Disconnect(connection);
        }

        public void SetCoreModule(IModule module)
        {
            if(CoreModule != null)
            {
                RemoveModule(CoreModule);
            }

            CoreModule = module;
            AddModule(module);
        }

        public IGraphReadOnly GetCoreSubgraph()
        {
            return new BreadthFirstTraversal().GetSubGraph(CoreModule, x => true);
        }
    }
}