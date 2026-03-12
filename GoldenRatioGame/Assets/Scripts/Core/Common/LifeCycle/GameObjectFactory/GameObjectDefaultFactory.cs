using UnityEngine;

namespace IM.Common
{
    public class GameObjectDefaultFactory : IGameObjectFactory
    {
        public GameObject Create(GameObject prefab)
        {
            return Object.Instantiate(prefab);
        }
    }
}