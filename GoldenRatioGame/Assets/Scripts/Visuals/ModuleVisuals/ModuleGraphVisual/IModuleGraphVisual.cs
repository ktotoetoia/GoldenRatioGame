using IM.Graphs;

namespace IM.Modules
{
    public interface IModuleGraphVisual
    {
        IModuleGraphReadOnly Source { get; set; }

        void RebuildSource();
    }
}