using IM.Graphs;

namespace IM.Visuals.Graph
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