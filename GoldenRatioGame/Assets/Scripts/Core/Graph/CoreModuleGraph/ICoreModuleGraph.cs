namespace IM.Graphs
{
    public interface ICoreModuleGraph : IModuleGraph
    {
        IModule CoreModule { get; }
        
        void SetCoreModule(IModule module);
        IGraphReadOnly GetCoreSubgraph();
    }
}