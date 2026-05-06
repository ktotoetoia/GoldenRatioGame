using System.Collections.Generic;
using IM.LifeCycle;
using UnityEngine;

namespace IM.Map
{
    public abstract class RoomFactory : ScriptableObject,IRoomFactory
    {
        public abstract IRoom Create(ISelectedRoomPattern roomPattern, IGameObjectFactory gameObjectFactory);
        public abstract IEnumerable<IRoomPattern> GetRoomPatterns();
    }
}