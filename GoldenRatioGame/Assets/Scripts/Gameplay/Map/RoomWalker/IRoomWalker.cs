using System.Collections.Generic;

namespace IM.Map
{
    public interface IRoomWalker
    {
        IEnumerable<IRoom> Available { get; }   
        IRoom Current { get; }
        void GoTo(IRoom room);
    }
}