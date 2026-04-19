using IM.Graphs;
using IM.Items;
using IM.Storages;

namespace IM.Modules
{
    public interface IModuleEditingContext : IModuleEditingContextReadOnly
    {
        bool IsUnsafe { get; }
        ICellFactoryStorage MutableStorage { get; }
        IConditionalCommandDataModuleGraph<IExtensibleItem> ModuleGraph { get; }
        IDataModule<IExtensibleItem> CreateModule(IExtensibleItem item);
        void SetUnsafe(bool value);
        bool AddToContext(IItem item);
        bool RemoveFromContext(IItem item);
    }
}