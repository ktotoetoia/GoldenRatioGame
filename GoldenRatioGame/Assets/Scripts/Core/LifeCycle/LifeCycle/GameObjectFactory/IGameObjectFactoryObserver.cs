using UnityEngine;

namespace IM.LifeCycle
{
    public interface IGameObjectFactoryObserver
    {
        void OnCreate(GameObject instance, bool deserialized);
    }
}