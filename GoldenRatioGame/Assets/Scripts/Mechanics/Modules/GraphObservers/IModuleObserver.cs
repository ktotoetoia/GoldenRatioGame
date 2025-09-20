using IM.Graphs;

namespace IM.Modules
{
    public interface IModuleObserver
    {
        void Add(IModule module);
        void Remove(IModule module);
    }
}