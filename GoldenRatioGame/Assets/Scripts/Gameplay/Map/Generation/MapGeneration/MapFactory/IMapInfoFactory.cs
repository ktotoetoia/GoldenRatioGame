using IM.LifeCycle;

namespace IM.Map.Grid
{
    public interface IMapInfoFactory
    {
        string AddresableAddress {get;}
        IMapInfo Create(IGameObjectFactory factory, int seed, int depth);
    }
}