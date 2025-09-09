using System;
using System.Collections.Generic;

namespace IM.Graphs
{
    public class CoreModuleGraphEvents : ICoreModuleGraph
    {
        private readonly ICoreModuleGraph _coreModuleGraph;

        public IModule CoreModule => _coreModuleGraph.CoreModule;
        public IEnumerable<IModuleConnection> Connections => _coreModuleGraph.Connections;
        public IEnumerable<IModule> Modules => _coreModuleGraph.Modules;
        public IReadOnlyList<INode> Nodes => _coreModuleGraph.Nodes;
        public IReadOnlyList<IEdge> Edges => _coreModuleGraph.Edges;

        public event Action<IModule> OnModuleAdded;
        public event Action<IModule> OnModuleRemoved;
        public event Action<IModuleConnection> OnModulesConnected;
        public event Action<IModule,IModule> OnModulesDisconnected;
        public event Action<IModule> OnCoreModuleSet;

        public CoreModuleGraphEvents(IModule coreModule) : this(new CoreModuleGraph(coreModule))
        {

        }

        public CoreModuleGraphEvents(ICoreModuleGraph coreModuleGraph)
        {
            _coreModuleGraph = coreModuleGraph;
        }

        public void AddModule(IModule module)
        {
            _coreModuleGraph.AddModule(module);

            OnModuleAdded?.Invoke(module);
        }

        public void RemoveModule(IModule module)
        {
            _coreModuleGraph.RemoveModule(module);

            OnModuleRemoved?.Invoke(module);
        }

        public IModuleConnection Connect(IModulePort output, IModulePort input)
        {
            IModuleConnection connection = _coreModuleGraph.Connect(output, input);
            
            OnModulesConnected?.Invoke(connection);

            return connection;
        }

        public void Disconnect(IModuleConnection connection)
        {
            IModule output = connection.Output.Module;
            IModule input = connection.Input.Module;

            _coreModuleGraph.Disconnect(connection);

            OnModulesDisconnected?.Invoke(output,input);
        }

        public IGraphReadOnly GetCoreSubgraph()
        {
            return _coreModuleGraph.GetCoreSubgraph();
        }

        public void SetCoreModule(IModule module)
        {
            _coreModuleGraph.SetCoreModule(module);

            OnCoreModuleSet?.Invoke(module);
        }
    }
}