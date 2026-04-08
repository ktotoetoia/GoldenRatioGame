using System.Collections.Generic;

namespace IM.Map
{
    public interface IRoomWalker
    {
        IEnumerable<IGameObjectRoom> Available { get; }   
        IGameObjectRoom Current { get; }
        
        void GoTo(IGameObjectRoom room);
    }
}