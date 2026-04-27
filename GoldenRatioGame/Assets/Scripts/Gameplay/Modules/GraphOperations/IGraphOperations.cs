using IM.Graphs;
using IM.Storages;

namespace IM.Modules
{
    public interface IGraphOperations<T> where T : class, IStorableReadOnly
    {
        IDataModuleGraphReadOnly<T> Graph { get; }
        GraphEditingService<T> GraphEditingService { get; }

        bool TryQuickAddModule(IDataModule<T> module);
        bool TryQuickRemoveModule();
        bool TryQuickRemoveModule(IDataModule<T> module);
    }
}