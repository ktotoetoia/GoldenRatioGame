using IM.Graphs;

namespace IM.Modules
{
    public interface IGraphEditingService<T> : IDataModuleGraphConditions<T>,IDataModuleGraphOperations<T>
    {
        IDataModuleGraphReadOnly<T> GraphReadOnly { get; }
        
        void Undo(int count);
        void Redo(int count);
        bool CanUndo(int count);
        bool CanRedo(int count);
    }
}