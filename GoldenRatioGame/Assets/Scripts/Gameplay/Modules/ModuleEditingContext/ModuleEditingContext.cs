using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.Items;
using IM.LifeCycle;
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
        public ITypeRegistry<object> ConvertableObjects { get; }
        public IDataModuleGraphReadOnly<IExtensibleItem> Graph => ModuleGraph;

        public ModuleEditingContext(ICellFactoryStorage storage,IConditionalCommandDataModuleGraph<IExtensibleItem> moduleGraph, CompositeDataModuleGraphConditions<IExtensibleItem> conditions, IEnumerable<object> convertableObjects)
        {
            _conditions = conditions;
            MutableStorage = storage;
            ModuleGraph = moduleGraph;
            ConvertableObjects = new TypeRegistry<object>(convertableObjects);
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

        public bool AddToContext(IItem item)
        {
            if (item.ItemState == ItemState.Hide|| item is not IStorable storable || Storage.ContainsItem(storable)) return false;
            
            MutableStorage.SetItem(MutableStorage.FirstOrDefault(x => x.Item == null) ?? MutableStorage.CreateCell(), storable);
            item.ItemState = ItemState.Hide;

            return true;
        }

        public bool RemoveFromContext(IItem item)
        {
            if (  item is not IStorable storable || !Storage.ContainsItem(storable)) return false;
            
            MutableStorage.ClearCell(MutableStorage.GetCell(storable));
            item.ItemState = ItemState.Show;
            return true;
        }
    }
}