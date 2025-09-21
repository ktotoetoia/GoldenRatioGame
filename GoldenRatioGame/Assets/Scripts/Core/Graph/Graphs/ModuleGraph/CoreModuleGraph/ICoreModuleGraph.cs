namespace IM.Graphs
{
    public interface ICoreModuleGraph :ICoreModuleGraphReadOnly, IModuleGraph
    {
        void SetCoreModule(IModule module);
    }
}