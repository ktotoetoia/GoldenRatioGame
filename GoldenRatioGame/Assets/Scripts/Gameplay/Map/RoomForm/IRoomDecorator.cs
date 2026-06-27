using IM.LifeCycle;

namespace IM.Map
{
    public interface IRoomDecorator
    {
        void Decorate(IGameObjectRoom room, IRoomShape shape, IGameObjectFactory factory);
    }
}