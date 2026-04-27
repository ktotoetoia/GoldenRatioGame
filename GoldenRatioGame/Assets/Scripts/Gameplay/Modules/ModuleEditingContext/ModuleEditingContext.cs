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
        public ITypeRegistry<object> Services { get; }
        public IReadOnlyStorage Storage { get; }
        public ITypeRegistry<object> Capabilities { get; }
        public IDataModuleGraphReadOnly<IExtensibleItem> Graph { get; }

        public ModuleEditingContext(IReadOnlyStorage storage,IDataModuleGraphReadOnly<IExtensibleItem> graph, IEnumerable<object> convertableObjects, IEnumerable<object> services)
        {
            Storage = storage;
            Graph = graph;
            Capabilities = new TypeRegistry<object>(convertableObjects);
            Services = new TypeRegistry<object>(services);
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
        
        public bool AddToContext(IItem item)
        {
            if (item.ItemState == ItemState.Hide|| item is not IStorable storable || Storage.ContainsItem(storable)) return false;
            
            return true;
        }

        public bool RemoveFromContext(IItem item)
        {
            if (  item is not IStorable storable || !Storage.ContainsItem(storable)) return false;
            
            
            return true;
        }
    }
}