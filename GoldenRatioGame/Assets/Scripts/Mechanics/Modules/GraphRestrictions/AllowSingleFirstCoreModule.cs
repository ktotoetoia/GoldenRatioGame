using System.Linq;
using IM.Graphs;

namespace IM.Modules
{
    public class AllowSingleFirstCoreModule : IModuleGraphConditions
    {
        private readonly IModuleGraph _graph;
        
        public AllowSingleFirstCoreModule(IModuleGraph graph)
        {
            _graph = graph;
        }

        public bool CanAddModule(IModule module)
        {
            return module is not ICoreExtensibleModule && _graph.Modules.Any(x => x is ICoreExtensibleModule) ||
                   module is ICoreExtensibleModule && !_graph.Modules.Any();
        }

        public bool CanRemoveModule(IModule module)
        {
            return module is not ICoreExtensibleModule || _graph.Modules.Count == 1;
        }

        public bool CanAddAndConnect(IModule module, IPort ownerPort, IPort targetPort)
        {
            return CanAddModule(module);
        }
    }
}