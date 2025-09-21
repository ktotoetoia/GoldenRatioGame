namespace IM.Graphs
{
    public interface ISafeModuleGraph : IModuleGraph
    {
        void Clear();
        void AddAndConnect(IModule toAdd, IModulePort addedPort, IModulePort targetPort);
    }
}