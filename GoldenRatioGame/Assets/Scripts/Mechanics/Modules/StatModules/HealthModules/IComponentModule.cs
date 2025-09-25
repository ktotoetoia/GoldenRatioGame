using IM.Graphs;

namespace IM.Modules
{
    public interface IComponentModule : IModule
    {
        T GetComponent<T>();
    }
}