using System.Collections.Generic;
using IM.LifeCycle;
using UnityEngine;

namespace IM.Map
{
    [CreateAssetMenu(menuName = "Map/Rectangle Room Factory")]
    public class RectangleRoomFactory : RoomFactory
    {
        [SerializeField] private GameObject _roomPrefab;
        [SerializeField] private GameObject _roomPortPrefab;
        [SerializeField] private List<EntityFactory> _entityFactories;
        [SerializeField] private List<GameObject> _gameObjects;
        [SerializeField] private Vector2Int _size = new(17, 9);

        public override IEnumerable<IRoomPattern> GetRoomPatterns()
        {
            return new List<IRoomPattern>
            {
                new RoomPattern(new Rect(Vector2.zero,_size),new Rect(Vector2.zero,_size),new List<IPortDefinition>(),new List<IPortDefinition>
                {
                    new PortDefinition(PortSide.West,0.5f),
                    new PortDefinition(PortSide.East,0.5f),
                    new PortDefinition(PortSide.North,0.5f),
                    new PortDefinition(PortSide.South,0.5f)
                })
            };
        }
        
        public override IRoom Create(ISelectedRoomPattern roomPattern, IGameObjectFactory gameObjectFactory)
        {            
            GameObject roomGO = gameObjectFactory.Create(_roomPrefab, false);
    
            IGameObjectRoom room = roomGO.GetComponent<IGameObjectRoom>();
            
            room.SetRect(roomPattern.RoomRect);
            
            foreach (IPortDefinition portDefinition in roomPattern.PortDefinitions)
            {
                GameObject portGameObject = gameObjectFactory.Create(_roomPortPrefab, false);
                RoomPort port = portGameObject.GetComponent<RoomPort>();
                port.PortSide = portDefinition.Side;
                port.NormalizedPosition = portDefinition.NormalizedPosition;
                port.Initialize(room);
                room.Add(port);
            }
    
            foreach (var prefab in _gameObjects) room.Add(gameObjectFactory.Create(prefab, false));
            foreach (var entityFactory in _entityFactories) room.Add(entityFactory.Create(gameObjectFactory).GameObject);

            return room;
        }
    }
}