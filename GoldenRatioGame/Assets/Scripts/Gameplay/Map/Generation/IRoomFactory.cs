using System.Collections.Generic;
using IM.LifeCycle;

namespace IM.Map
{
    public interface IRoomFactory : IFactory<IRoom,ISelectedRoomPattern, IGameObjectFactory>
    {
        IEnumerable<IRoomPattern> GetRoomPatterns();
    }
}