using IM.Graphs;

namespace IM.Modules
{
    public interface IComponentModule : IModule
    {
        T GetComponent<T>();
        bool TryGetComponent<T>(out T component);
    }
}