using IM.Graphs;
using IM.Storages;

namespace IM.Modules
{
    public class ModuleEditingContext : IModuleEditingContext
    {
        private readonly  CompositeDataModuleGraphConditions<IExtensibleItem> _conditions;

        public bool IsUnsafe { get; private set; }
        public ICellFactoryStorage MutableStorage { get; }
        public IConditionalCommandDataModuleGraph<IExtensibleItem> ModuleGraph { get; }
        public IReadOnlyStorage Storage => MutableStorage;
        public IDataModuleGraphReadOnly<IExtensibleItem> Graph => ModuleGraph;

        public ModuleEditingContext(ICellFactoryStorage storage,IConditionalCommandDataModuleGraph<IExtensibleItem> moduleGraph, CompositeDataModuleGraphConditions<IExtensibleItem> conditions)
        {
            _conditions = conditions;
            MutableStorage = storage;
            ModuleGraph = moduleGraph;
        }

        public IDataModule<IExtensibleItem> CreateModule(IExtensibleItem item)
        {
            DataModule<IExtensibleItem> dataModule = new DataModule<IExtensibleItem>(item);

            foreach (IDataPort<IExtensibleItem> port in item.PortFactory.Create(dataModule))
            {
                dataModule.AddPort(port);   
            }
            
            return dataModule;
        }
        
        public void SetUnsafe(bool value)
        {
            IsUnsafe = value;
            
            _conditions.Disable = IsUnsafe;
        }
    }
}