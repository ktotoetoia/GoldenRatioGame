using System.Collections.Generic;
using UnityEngine;

namespace IM.Map
{
    public interface IRoom
    {
        IEnumerable<IRoomVisitor> RoomVisitors { get; }
        IEnumerable<IRoomPort> RoomPorts { get; }
        
        bool IsActive { get; }
        bool Add(IRoomVisitor roomVisitor);
        bool Remove(IRoomVisitor roomVisitor);
        bool Add(IRoomPort roomPort);
        bool Remove(IRoomPort roomPort);
    }
}