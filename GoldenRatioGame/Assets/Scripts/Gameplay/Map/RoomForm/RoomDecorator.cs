using System.Collections.Generic;
using IM.LifeCycle;
using UnityEngine;

namespace IM.Map
{
    public abstract class RoomDecorator : ScriptableObject, IRoomDecorator
    {
        public abstract IEnumerable<GameObject> Decorate(IGameObjectRoom room, IRoomShape shape, IGameObjectFactory factory);
    }
}