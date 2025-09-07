using IM.Graphs;

namespace IM.Modules
{
    public interface IGraphReader
    {
        IModuleGraph Graph { get; }

        void OnGraphStructureChanged();
    }
}