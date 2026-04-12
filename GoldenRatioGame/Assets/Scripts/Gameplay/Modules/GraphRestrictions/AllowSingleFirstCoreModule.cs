using System.Linq;
using IM.Graphs;

namespace IM.Modules
{
    public class AllowSingleFirstCoreModule<T> : IDataModuleGraphConditions<T>
    {
        private readonly IDataModuleGraphReadOnly<T> _graph;
        
        public AllowSingleFirstCoreModule(IDataModuleGraphReadOnly<T> graph)
        {
            _graph = graph;
        }

        public bool CanAdd(IDataModule<T> module)
        {
            return module.Value is not ICoreExtensibleItem && _graph.DataModules.Any(x => x.Value is ICoreExtensibleItem) ||
                   module.Value is ICoreExtensibleItem && !_graph.Modules.Any();
        }

        public bool CanRemove(IDataModule<T> module)
        {
            return module.Value is not ICoreExtensibleItem || _graph.Modules.Count == 1;
        }

        public bool CanAddAndConnect(IDataModule<T> module, IDataPort<T> ownerPort, IDataPort<T> targetPort)
        {
            return CanAdd(module);
        }
    }
}