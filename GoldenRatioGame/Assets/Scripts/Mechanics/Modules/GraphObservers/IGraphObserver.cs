using IM.Graphs;

namespace IM.Modules
{
    public interface IGraphObserver
    {
        ICoreModuleGraph Graph { get; }
        void OnGraphChange();
    }
}