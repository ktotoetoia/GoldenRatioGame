using System;
using UnityEngine;

namespace IM.Map
{
    public interface IGameObjectRoomEvents
    {
        event Action<GameObject> GameObjectAdded;
        event Action<GameObject> GameObjectRemoved;
        event Action<IRoomPort> RoomPortAdded;
        event Action<IRoomPort> RoomPortRemoved;
    }
}