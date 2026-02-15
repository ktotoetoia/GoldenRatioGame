using IM.Graphs;

namespace IM.Visuals.Graph
{
    public interface IGraphOperations
    {
        IConditionalCommandModuleGraph Graph { get; }
        
        bool QuickAddModule(IModule module);
        void QuickRemoveModule();
        void Undo(int count);
        void Redo(int count);
    }
}