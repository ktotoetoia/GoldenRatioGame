using System.Linq;
using IM.Graphs;

namespace IM.Modules
{
    public class DisallowSameItem<T> : IDataModuleGraphConditions<T>
    {
        private readonly IDataModuleGraphReadOnly<T> _graph;

        public DisallowSameItem(IDataModuleGraphReadOnly<T> graph)
        {
            _graph = graph;
        }
        
        public bool CanAdd(IDataModule<T> module)
        {
            return !_graph.DataModules.Any(x => x.Value.Equals(module.Value));
        }

        public bool CanAddAndConnect(IDataModule<T> module, IDataPort<T> ownerPort, IDataPort<T> targetPort)
        {
            return CanAdd(module);
        }
    }
}