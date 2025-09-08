using IM.Graphs;

namespace IM.Modules
{
    public interface IGraphReader
    {
        ICoreModuleGraph Graph { get; }

        void OnGraphStructureChanged();
    }
}