using System;

namespace IM.Map
{
    public interface IRoomEvents
    {
        event Action<IRoomVisitor> RoomVisitorAdded;
        event Action<IRoomVisitor> RoomVisitorRemoved;
        event Action<IRoomPort> RoomPortAdded;
        event Action<IRoomPort> RoomPortRemoved;
        event Action<IRoomWalker> RoomWalkerEnter;
        event Action<IRoomWalker> RoomWalkerExit;
    }
}