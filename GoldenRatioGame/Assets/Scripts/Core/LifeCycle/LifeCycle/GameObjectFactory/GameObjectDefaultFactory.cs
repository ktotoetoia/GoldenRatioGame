using UnityEngine;

namespace IM.LifeCycle
{
    public class GameObjectDefaultFactory : IGameObjectFactory
    {
        public GameObject Create(GameObject prefab,bool deserialized)
        {
            return Object.Instantiate(prefab);
        }
    }
}