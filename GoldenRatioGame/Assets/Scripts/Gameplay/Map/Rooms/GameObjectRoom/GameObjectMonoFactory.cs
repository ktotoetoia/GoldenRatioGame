using IM.LifeCycle;
using UnityEngine;

namespace IM.Map
{
    public class GameObjectRoomMonoFactory : IRoomFactory
    {
        private readonly IGameObjectFactory _factory;
        private readonly GameObject _prefab;
        
        public GameObjectRoomMonoFactory(IGameObjectFactory factory,GameObject prefab)
        {
            _factory = factory;
            _prefab = prefab;
        }
        
        public IGameObjectRoom Create()
        {
            GameObject created = _factory.Create(_prefab,false);

            return created.GetComponent<IGameObjectRoom>();
        }
    }
}