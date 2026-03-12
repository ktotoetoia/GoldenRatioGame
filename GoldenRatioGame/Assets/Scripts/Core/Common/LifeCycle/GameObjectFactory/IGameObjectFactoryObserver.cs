using UnityEngine;

namespace IM.Common
{
    public interface IGameObjectFactoryObserver
    {
        void OnCreate(GameObject instance);
    }
}