using IM.Graphs;

namespace IM.Modules
{
    public interface IGraphOperations
    {
        IConditionalCommandModuleGraph Graph { get; }
        
        bool TryQuickAddModule(IModule module);
        bool TryQuickRemoveModule();
        bool TryQuickRemoveModule(IModule module);
        void Undo(int count);
        void Redo(int count);
    }
}