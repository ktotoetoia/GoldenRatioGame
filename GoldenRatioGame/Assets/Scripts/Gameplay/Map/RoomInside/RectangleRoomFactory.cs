using System.Collections.Generic;
using IM.LifeCycle;
using UnityEngine;

namespace IM.Map
{
    [CreateAssetMenu(menuName = "Map/Rectangle Room Factory")]
    public class RectangleRoomFactory : RoomFactory
    {
        [SerializeField] private GameObject _roomPrefab;
        [SerializeField] private List<EntityFactory> _entityFactories;
        [SerializeField] private List<GameObject> _gameObjects;
        [SerializeField] private Vector2Int _minSize = new(8, 8);
        [SerializeField] private Vector2Int _maxSize = new(16, 16);

        public override IGameObjectRoom Create(IGameObjectFactory factory)
        {
            GameObject roomGO = factory.Create(_roomPrefab, false);
        
            if (roomGO.TryGetComponent(out RectangleRoomAspect aspect))
            {
                aspect.SetSize(new Vector2Int(
                    Random.Range(_minSize.x, _maxSize.x), 
                    Random.Range(_minSize.y, _maxSize.y)));
            }

            IGameObjectRoom room = roomGO.GetComponent<IGameObjectRoom>();

            foreach (var prefab in _gameObjects) room.Add(factory.Create(prefab, false));
            foreach (var entityFactory in _entityFactories) room.Add(entityFactory.Create(factory).GameObject);

            return room;
        }
    }
}