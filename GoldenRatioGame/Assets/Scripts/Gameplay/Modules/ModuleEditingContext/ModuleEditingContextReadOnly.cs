using System.Collections.Generic;
using IM.Graphs;
using IM.LifeCycle;
using IM.Storages;

namespace IM.Modules
{
    public class ModuleEditingContextReadOnly : IModuleEditingContextReadOnly 
    {
        public IDataModuleGraphReadOnly<IExtensibleItem> Graph { get; }
        public IReadOnlyStorage Storage { get; }
        public ITypeRegistry<object> Capabilities { get; }

        public ModuleEditingContextReadOnly(IDataModuleGraphReadOnly<IExtensibleItem> moduleGraph = null, IReadOnlyStorage storage= null, IEnumerable<object> convertableObjects= null)
        {
            
            
            Graph = moduleGraph ?? new DataModuleGraphReadOnly<IExtensibleItem>();
            Storage = storage ?? new ReadOnlyStorage();
            Capabilities = new TypeRegistry<object>(convertableObjects ?? new List<object>());
        }
    }
}