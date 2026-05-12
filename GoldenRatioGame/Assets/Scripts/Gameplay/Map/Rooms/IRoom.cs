using System.Collections.Generic;
using UnityEngine;

namespace IM.Map
{
    public interface IRoom
    {
        Rect Rect { get; }
        
        IEnumerable<IRoomPort> RoomPorts { get; }
        bool IsActive { get; }
        bool Add(IRoomPort roomPort);
        bool Remove(IRoomPort roomPort);
        
        void SetRect(Rect rect);
    }
}