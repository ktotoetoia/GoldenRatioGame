using IM.Graphs;
using IM.ModuleGraph;

namespace IM.Modules
{
    public interface IModuleGraphVisual
    {
        IVisualModuleGraph Source { get; set; }
    }
}