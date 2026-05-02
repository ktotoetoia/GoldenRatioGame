using UnityEngine;

namespace IM.LifeCycle
{
    public abstract class EntityFactory : ScriptableObject
    {
        public abstract IEntity Create(IGameObjectFactory gameObjectFactory);
    }
}