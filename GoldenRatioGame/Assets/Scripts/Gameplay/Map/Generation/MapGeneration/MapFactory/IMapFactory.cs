using IM.LifeCycle;

namespace IM.Map.Grid
{
    public interface IMapFactory
    {
        IMapInfo Create(IGameObjectFactory factory, int seed, int depth);
    }
}