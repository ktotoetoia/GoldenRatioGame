using UnityEngine;

namespace IM.LifeCycle
{
    public interface IGameObjectFactory : IFactory<GameObject,GameObject,bool>
    {
    }
}