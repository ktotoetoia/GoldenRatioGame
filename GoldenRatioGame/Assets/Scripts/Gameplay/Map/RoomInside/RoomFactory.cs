using IM.LifeCycle;
using UnityEngine;

namespace IM.Map
{
    public abstract class RoomFactory : ScriptableObject
    {
        public abstract IGameObjectRoom Create(IGameObjectFactory gameObjectFactory);
    }
}