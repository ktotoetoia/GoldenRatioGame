using IM.Graphs;

namespace IM.Modules
{
    public interface ICoreModuleGraph : IModuleGraph
    {
        IModule CoreModule { get; }
        
        void SetCoreModule(IModule module);
        IGraphReadOnly GetCoreSubgraph();
    }
}