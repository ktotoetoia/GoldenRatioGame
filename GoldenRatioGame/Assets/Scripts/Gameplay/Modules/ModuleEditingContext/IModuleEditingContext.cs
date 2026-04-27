using System;
using IM.Graphs;
using IM.Items;
using IM.LifeCycle;

namespace IM.Modules
{
    public interface IModuleEditingContext : IModuleEditingContextReadOnly
    {
        ITypeRegistry<object> Services { get; } 
        IDataModule<IExtensibleItem> CreateModule(IExtensibleItem item);
        bool AddToContext(IItem item);
        bool RemoveFromContext(IItem item);
    }
}