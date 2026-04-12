using System.Collections.Generic;
using IM.Graphs;
using IM.LifeCycle;

namespace IM.Modules
{
    public class ConditionsFactory : IFactory<IEnumerable<IDataModuleGraphConditions<IExtensibleItem>>,
        IDataModuleGraphReadOnly<IExtensibleItem>>
    {
        public IEnumerable<IDataModuleGraphConditions<IExtensibleItem>> Create(IDataModuleGraphReadOnly<IExtensibleItem> graph)
        {
            return new List<IDataModuleGraphConditions<IExtensibleItem>>()
            {
                new DefaultDataModuleGraphConditions<IExtensibleItem>(graph),
                new DisallowSameItem<IExtensibleItem>(graph),
                new AllowSingleFirstCoreModule<IExtensibleItem>(graph),
                new AllowConnectionIfPortsMatch<IExtensibleItem>()
            };
        }
    }
}