using IM.Graphs;

namespace IM.Visuals.Graph
{
    public interface IGraphOperations
    {
        bool QuickAddModule(IModule module);
        void QuickRemoveModule();
        void Undo(int count);
        void Redo(int count);
    }
}