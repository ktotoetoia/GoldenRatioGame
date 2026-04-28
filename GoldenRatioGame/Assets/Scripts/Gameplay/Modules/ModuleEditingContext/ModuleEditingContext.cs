using System;
using IM.Graphs;
using IM.LifeCycle;
using IM.Storages;

namespace IM.Modules
{
    public class ModuleEditingContext : IModuleEditingContext
    {
        private readonly TypeRegistry<object> _capabilities = new();
        private readonly TypeRegistry<IEditingService> _services = new();
        
        public IReadOnlyStorage Storage { get; }
        public IDataModuleGraphReadOnly<IExtensibleItem> Graph { get; }
        public IGraphEditingService<IExtensibleItem> GraphEditing { get; }
        public IGraphEditingService<IExtensibleItem> UnsafeGraphEditing { get; }
        public IStorageEditingService StorageEditing { get; }
        public ITypeRegistry<object> Capabilities => _capabilities;
        public ITypeRegistry<IEditingService> Services => _services;
        
        public ModuleEditingContext(IConditionalCommandDataModuleGraph<IExtensibleItem> graph,
            CompositeDataModuleGraphConditions<IExtensibleItem> conditions,
            ICellFactoryStorage storage)
        {
            Storage = storage;
            Graph = graph;
            GraphEditing = new GraphEditingService(graph,storage);
            UnsafeGraphEditing = new UnsafeGraphEditingService<IExtensibleItem>(GraphEditing,conditions);
            StorageEditing = new StorageEditingService(storage);
        }
        
        public ModuleEditingContext(IConditionalCommandDataModuleGraph<IExtensibleItem> graph,
            ICellFactoryStorage storage, 
            IGraphEditingService<IExtensibleItem>  graphEditing,
            IGraphEditingService<IExtensibleItem>  unsafeGraphEditing,
            IStorageEditingService storageEditing)
        {
            Storage = storage;
            Graph = graph;
            GraphEditing = graphEditing;
            UnsafeGraphEditing = unsafeGraphEditing;
            StorageEditing = storageEditing;
        }

        public bool AddCapability(object capability)
        {
            if (capability is IEditingService) throw new ArgumentException("Services cannot be added as capability");
            
            return _capabilities.Add(capability);
        }
        
        public bool AddService(IEditingService service)
        {
            return _services.Add(service);
        }
    }
}