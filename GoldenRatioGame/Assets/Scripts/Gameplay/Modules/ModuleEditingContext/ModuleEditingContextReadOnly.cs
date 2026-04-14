using System.Collections.Generic;
using IM.Graphs;
using IM.Storages;

namespace IM.Modules
{
    public class ModuleEditingContextReadOnly : IModuleEditingContextReadOnly 
    {
        public IDataModuleGraphReadOnly<IExtensibleItem> Graph { get; }
        public IReadOnlyStorage Storage { get; }
        
        public ModuleEditingContextReadOnly()
        {
            Graph = new DataModuleGraphReadOnly<IExtensibleItem>(new List<IDataModule<IExtensibleItem>>(),new List<IDataConnection<IExtensibleItem>>());
            Storage = new ReadOnlyStorage();
        }
        
        public ModuleEditingContextReadOnly(IDataModuleGraphReadOnly<IExtensibleItem> moduleGraph, IReadOnlyStorage storage)
        {
            Graph = moduleGraph;
            Storage = storage;
        }
    }
}