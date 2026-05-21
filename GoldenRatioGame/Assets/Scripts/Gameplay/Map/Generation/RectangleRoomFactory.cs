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
        [SerializeField] private List<Vector2Int> _roomPositions = new()
        {
            new Vector2Int(1, 1),
            new Vector2Int(1,2),
            new Vector2Int(2, 1),
            new Vector2Int(2, 2),
        };

        public override IEnumerable<IRoomPattern> GetRoomPatterns()
        {
            List<IRoomPattern> patterns = new List<IRoomPattern>();
            RectangleRoomPatternFactory patternFactory = new (); 
            
            foreach (Vector2Int roomPattern in _roomPositions)
            {
                patterns.Add(patternFactory.Create(_size, roomPattern));
            }

            return patterns;
        }
        
        public override IRoom Create(ISelectedRoomPattern roomPattern, IGameObjectFactory gameObjectFactory)
        {            
            GameObject roomGO = gameObjectFactory.Create(_roomPrefab, false);
            IGameObjectRoom room = roomGO.GetComponent<IGameObjectRoom>();
            
            room.SetRect(roomPattern.Shape.CellRect);

            foreach (var kvp in roomPattern.PortDefinitions)
            {
                Vector2Int cellOffset = kvp.Key;
                foreach (IPortDefinition portDefinition in kvp.Value)
                {
                    GameObject portGameObject = gameObjectFactory.Create(_roomPortPrefab, false);
                    RoomPort port = portGameObject.GetComponent<RoomPort>();
                    port.PortSide = portDefinition.Side;
                    port.CellOffset = cellOffset;

                    float defaultNorm = portDefinition.NormalizedPosition == 0 ? 0.5f : portDefinition.NormalizedPosition;
        
                    if (portDefinition.Side is PortSide.North or PortSide.South)
                    {
                        float localX = (cellOffset.x * _size.x) + (defaultNorm * _size.x);
                        port.NormalizedPosition = localX / roomPattern.Shape.CellRect.width;
                    }
                    else
                    {
                        float localY = (cellOffset.y * _size.y) + (defaultNorm * _size.y);
                        port.NormalizedPosition = localY / roomPattern.Shape.CellRect.height;
                    }

                    port.Initialize(room);
                    room.Add(port);
                }
            }

            foreach (var prefab in _gameObjects) room.Add(gameObjectFactory.Create(prefab, false));
            foreach (var entityFactory in _entityFactories) room.Add(entityFactory.Create(gameObjectFactory).GameObject);

            return room;
        }
    }
}