using System;
using System.Collections.Generic;

namespace IM.Graphs
{
    public class CoreModuleGraph : ICoreModuleGraph
    {
        private readonly IModuleGraph _moduleGraph;
        public IReadOnlyList<INode> Nodes => _moduleGraph.Nodes;
        public IReadOnlyList<IEdge> Edges => _moduleGraph.Edges;
        public IReadOnlyList<IConnection> Connections => _moduleGraph.Connections;
        public IReadOnlyList<IModule> Modules => _moduleGraph.Modules;
        public IModule CoreModule { get; private set; }

        public CoreModuleGraph(IModule coreModule) :this(coreModule,new SafeModuleGraph())
        {
            
        }
        
        public CoreModuleGraph(IModule coreModule, IModuleGraph moduleGraph)
        {
            _moduleGraph = moduleGraph;
            SetCoreModule(coreModule);
        }
        
        public bool AddModule(IModule module)
        {
            return _moduleGraph.AddModule(module);
        }

        public bool RemoveModule(IModule module)
        {
            if (module == CoreModule)
                throw new Exception("cannot remove CoreModule");
            
            return _moduleGraph.RemoveModule(module);
        }

        public IConnection Connect(IModulePort output, IModulePort input)
        {
            return _moduleGraph.Connect(output, input);
        }

        public void Disconnect(IConnection connection)
        {
            _moduleGraph.Disconnect(connection);
        }

        public void SetCoreModule(IModule module)
        {
            if(CoreModule != null)
            {
                _moduleGraph.RemoveModule(module);
            }

            CoreModule = module;
            AddModule(module);
        }
    }
}