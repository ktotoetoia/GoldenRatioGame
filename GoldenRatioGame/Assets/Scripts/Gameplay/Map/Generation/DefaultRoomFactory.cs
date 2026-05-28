using System.Collections.Generic;
using IM.LifeCycle;
using UnityEngine;

namespace IM.Map
{
    [CreateAssetMenu(menuName = "Map/Room Factories/Rectangle Room Factory")]
    public class DefaultRoomFactory : RoomFactory
    {
        [SerializeField] private GameObject _roomPrefab;
        [SerializeField] private GameObject _roomPortPrefab;
        [SerializeField] private List<EntityFactory> _entityFactories;
        [SerializeField] private List<GameObject> _gameObjects;
        [SerializeField] private Vector2Int _size = new(17, 9);
        [SerializeField] private List<Vector2Int> _roomSizes = new()
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
            
            foreach (Vector2Int roomPattern in _roomSizes)
            {
                patterns.Add(patternFactory.Create(_size, roomPattern));
            }
            
            patterns.Add(new OffsetRoomPatternFactory().Create(_size, new List<Vector2Int>(){new(0,0),new(1,0),new(1,1)}));

            return patterns;
        }
        
        public override IRoom Create(ISelectedRoomPattern roomPattern, IGameObjectFactory gameObjectFactory)
        {
            GameObject roomGO = gameObjectFactory.Create(_roomPrefab, false);
            IGameObjectRoom room = roomGO.GetComponent<IGameObjectRoom>();

            if (roomGO.TryGetComponent(out IRoomForm roomForm))
                roomForm.Apply(roomPattern.Shape);
            else
                Debug.LogWarning($"No IRoomForm found on prefab: {_roomPrefab}");

            foreach (var (cellOffset, ports) in roomPattern.PortDefinitions)
            {
                foreach (IPortDefinition portDefinition in ports)
                {
                    GameObject portGO = gameObjectFactory.Create(_roomPortPrefab, false);
                    RoomPort port = portGO.GetComponent<RoomPort>();

                    float normalized = portDefinition.NormalizedPosition == 0f ? 0.5f : portDefinition.NormalizedPosition;

                    port.Initialize(room, new PortIdentity(portDefinition.Index, normalized, cellOffset, portDefinition.Side));
                    room.Add(port);
                }
            }

            foreach (var prefab in _gameObjects) room.Add(gameObjectFactory.Create(prefab, false));
            foreach (var factory in _entityFactories) room.Add(factory.Create(gameObjectFactory).GameObject);

            return room;
        }
    }
}