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
        [SerializeField] private Vector2Int _size = new(16, 16);

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

            if (roomGO.TryGetComponent(out RectangleRoomForm form))
            {
                form.SetSize(new Vector2Int((int)roomPattern.RoomRect.size.x, (int)roomPattern.RoomRect.size.y)); 
            }
    
            IGameObjectRoom room = roomGO.GetComponent<IGameObjectRoom>();

            foreach (IPortDefinition portDefinition in roomPattern.PortDefinitions)
            {
                GameObject portGameObject = gameObjectFactory.Create(_roomPortPrefab, false);
                RoomPort port = portGameObject.GetComponent<RoomPort>();
                port.PortSide = portDefinition.Side;
                port.Initialize(room);
                room.Add(port);
                
                portGameObject.transform.localPosition = ResolvePortLocalPosition(roomPattern.CellRect.size, portDefinition);
            }
    
            foreach (var prefab in _gameObjects) room.Add(gameObjectFactory.Create(prefab, false));
            foreach (var entityFactory in _entityFactories) room.Add(entityFactory.Create(gameObjectFactory).GameObject);

            return room;
        }

        private Vector2 ResolvePortLocalPosition(Vector2 size, IPortDefinition port)
        {
            float halfW = size.x / 2f;
            float halfH = size.y / 2f;

            return port.Side switch
            {
                PortSide.North => new Vector2(Mathf.Lerp(-halfW, halfW, port.NormalizedPosition),  halfH),
                PortSide.South => new Vector2(Mathf.Lerp(-halfW, halfW, port.NormalizedPosition), -halfH),
                PortSide.East  => new Vector2( halfW, Mathf.Lerp(-halfH, halfH, port.NormalizedPosition)),
                PortSide.West  => new Vector2(-halfW, Mathf.Lerp(-halfH, halfH, port.NormalizedPosition)),
                _ => Vector2.zero
            };
        }
    }
}