using IM.LifeCycle;

namespace IM.Map
{
    public interface IRoomVisitor
    {
        IEntity Entity { get; }
        IRoom CurrentRoom { get; set; }
        bool ActiveInRoom { get; set; }
    }
}