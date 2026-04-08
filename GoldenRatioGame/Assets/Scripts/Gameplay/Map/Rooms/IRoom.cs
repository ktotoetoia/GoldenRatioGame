using System.Collections.Generic;

namespace IM.Map
{
    public interface IRoom
    {
        IEnumerable<IRoomPort> RoomPorts { get; }
        bool IsActive { get; }
        bool Add(IRoomPort roomPort);
        bool Remove(IRoomPort roomPort);
    }
}