using System.Collections.Generic;

namespace IM.Map
{
    public interface IRoom
    {
        IEnumerable<IRoomVisitor> RoomVisitors { get; }
        IEnumerable<IRoomWalker> RoomWalkers { get; }
        bool IsActive { get; }
        bool Add(IRoomVisitor roomVisitor);
        bool Remove(IRoomVisitor roomVisitor);
        void Enter(IRoomWalker roomWalker);
        void Exit(IRoomWalker roomWalker);
    }
}