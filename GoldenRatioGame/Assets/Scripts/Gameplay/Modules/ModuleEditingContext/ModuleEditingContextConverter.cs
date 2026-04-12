using System.Collections.Generic;
using IM.Graphs;
using IM.LifeCycle;
using IM.Storages;

namespace IM.Modules
{
    public class ModuleEditingContextConverter : IModuleEditingContextConverter
    {
        private readonly IFactory<IEnumerable<IDataModuleGraphConditions<IExtensibleItem>>, IDataModuleGraphReadOnly<IExtensibleItem>> _conditionsFactory;
        private readonly IDataModuleGraphCloner<IDataModuleGraphReadOnly<IExtensibleItem>, IExtensibleItem> _dataModuleGraphCloner;
        
        public ModuleEditingContextConverter(IFactory<IEnumerable<IDataModuleGraphConditions<IExtensibleItem>>, IDataModuleGraphReadOnly<IExtensibleItem>> conditionsFactory)
        {
            _conditionsFactory = conditionsFactory;
            _dataModuleGraphCloner = new DataModuleGraphReadOnlyCloner<IExtensibleItem>();
        }
        
        public IModuleEditingContextReadOnly ToReadOnly(IModuleEditingContext test)
        {
            IReadOnlyStorage storage = new ReadOnlyStorage(test.MutableStorage);
            IDataModuleGraphReadOnly<IExtensibleItem>  moduleGraph = _dataModuleGraphCloner.Copy(test.ModuleGraph,x=>x);
            
            return new ModuleEditingContextReadOnly(moduleGraph, storage);
        }

        public IModuleEditingContext ToMutable(IModuleEditingContextReadOnly test)
        {
            ICellFactoryStorage storage = new CellFactoryStorage(test.Storage);

            ICommandDataModuleGraph<IExtensibleItem> commandDataModuleGraph =
                new CommandDataModuleGraph<IExtensibleItem>();
            _dataModuleGraphCloner.Apply(test.Graph, commandDataModuleGraph,x => x);
            
            var conditions = new CompositeDataModuleGraphConditions<IExtensibleItem>(_conditionsFactory.Create(commandDataModuleGraph));
            var a = new ConditionalCommandDataModuleGraph<IExtensibleItem>(commandDataModuleGraph,conditions);

            return new ModuleEditingContext(storage, a,conditions);
        }
    }
}