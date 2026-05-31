using IM.LifeCycle;

namespace IM.Map
{
    public interface IRoomForm
    {
        IRoomShape RoomShape { get; }
        void Apply(IRoomShape shape);
    }
    public interface IRoomDecorator
    {
        void Decorate(IGameObjectRoom room, IRoomShape shape, IGameObjectFactory factory);
    }
}