using IM.Graphs;

namespace IM.Modules
{
    public interface IGraphEditingService<T> : IDataModuleGraphConditions<T>,IDataModuleGraphOperations<T>, IEditingService
    {
        IDataModuleGraphReadOnly<T> GraphReadOnly { get; }
        IDataModule<T> CreateModule(T item);
    }
}