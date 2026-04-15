using System.Collections.Generic;
using IM.Graphs;
using IM.LifeCycle;
using IM.Storages;

namespace IM.Modules
{
    public class DataModuleGraphConditionsFactory : IFactory<IEnumerable<IDataModuleGraphConditions<IExtensibleItem>>,
        IDataModuleGraphReadOnly<IExtensibleItem>,IReadOnlyStorage>
    {
        public IEnumerable<IDataModuleGraphConditions<IExtensibleItem>> Create(IDataModuleGraphReadOnly<IExtensibleItem> graph, IReadOnlyStorage storage)
        {
            return new List<IDataModuleGraphConditions<IExtensibleItem>>()
            {
                new DefaultDataModuleGraphConditions<IExtensibleItem>(graph),
                new DisallowSameItem<IExtensibleItem>(graph),
                new AllowSingleFirstCoreModule<IExtensibleItem>(graph),
                new AllowConnectionIfPortsMatch<IExtensibleItem>(),
                new AllowAddIfStorageContains<IExtensibleItem>(storage),
            };
        }
    }
}