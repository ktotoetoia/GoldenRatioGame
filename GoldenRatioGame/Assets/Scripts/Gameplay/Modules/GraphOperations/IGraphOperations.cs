using IM.Graphs;

namespace IM.Modules
{
    public interface IGraphOperations<T>
    {
        IConditionalCommandDataModuleGraph<T> Graph { get; }
        
        bool TryQuickAddModule(IDataModule<T> module);
        bool TryQuickRemoveModule();
        bool TryQuickRemoveModule(IDataModule<T> module);
        void Undo(int count);
        void Redo(int count);
    }
}