namespace IM.Graphs
{
    public interface IModuleObserver
    {
        void Add(IModule module);
        void Remove(IModule module);
    }
}