using UnityEngine;
using UnityEngine.Tilemaps;

namespace IM.Map
{
    [DefaultExecutionOrder(-100)]
    [RequireComponent(typeof(GameObjectRoomMono))]
    public class RectangleRoomForm : MonoBehaviour, IRoomForm
    {
        [Header("Tilemap Settings")]
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private TileBase _floorTile;
        [SerializeField] private TileBase _wallTile;
        [SerializeField] private int _portTileRadius = 1;
        private GameObjectRoomMono _gameObjectRoomMono;
        private const int WallBorder = 1;

        public IRoomShape RoomShape { get; private set; }

        private void Awake()
        {
            _gameObjectRoomMono = GetComponent<GameObjectRoomMono>();
            _gameObjectRoomMono.RoomPortAdded += SetUpRoomPort;
        }

        private void OnDestroy()
        {
            if (_gameObjectRoomMono != null) 
            {
                _gameObjectRoomMono.RoomPortAdded -= SetUpRoomPort;
            }
        }

        public void Apply(IRoomShape shape)
        {
            if (shape == null) return;

            RoomShape = shape;

            GenerateRoom(shape);

            if (_gameObjectRoomMono?.RoomPorts == null) return;
            foreach (IRoomPort port in _gameObjectRoomMono.RoomPorts) 
            {
                SetUpRoomPort(port);
            }
        }

        private void GenerateRoom(IRoomShape shape)
        {
            if (!_tilemap || !_floorTile || !_wallTile) return;
            _tilemap.ClearAllTiles();

            DrawTiles(shape, _wallTile, WallBorder);
            DrawTiles(shape, _floorTile, 0);
        }

        private void DrawTiles(IRoomShape shape, TileBase tileToSet, int border)
        {
            foreach (Vector2Int offset in shape.Offsets)
            {
                var (startX, startY) = RoomShape.Metrics.GetCellOrigin(offset);
                
                int minX = startX - border;
                int maxX = startX + RoomShape.Metrics.CellW + border;
                int minY = startY - border;
                int maxY = startY + RoomShape.Metrics.CellH + border;

                for (int x = minX; x < maxX; x++)
                {
                    for (int y = minY; y < maxY; y++)
                    {
                        _tilemap.SetTile(new Vector3Int(x, y, 0), tileToSet);
                    }
                }
            }
        }

        private void SetUpRoomPort(IRoomPort roomPort)
        {
            if (roomPort is not MonoBehaviour mb || !RoomShape.Metrics.IsValid) return;
            ResolvePortPosition(mb, roomPort.PortIdentity);
        }

        private void ResolvePortPosition(MonoBehaviour mb, IPortIdentity identity)
        {
            var (cellStartX, cellStartY) = RoomShape.Metrics.GetCellOrigin(identity.CellOffset);

            int wallLeft   = cellStartX - WallBorder;
            int wallRight  = cellStartX + RoomShape.Metrics.CellW - 1 + WallBorder;
            int wallBottom = cellStartY - WallBorder;
            int wallTop    = cellStartY + RoomShape.Metrics.CellH - 1 + WallBorder;

            float norm = identity.NormalizedPosition;

            Vector3Int tilePos = identity.PortSide switch
            {
                PortSide.North => new(Mathf.RoundToInt(Mathf.Lerp(wallLeft, wallRight, norm)), wallTop,    0),
                PortSide.South => new(Mathf.RoundToInt(Mathf.Lerp(wallLeft, wallRight, norm)), wallBottom, 0),
                PortSide.East  => new(wallRight,  Mathf.RoundToInt(Mathf.Lerp(wallBottom, wallTop, norm)), 0),
                PortSide.West  => new(wallLeft,   Mathf.RoundToInt(Mathf.Lerp(wallBottom, wallTop, norm)), 0),
                _              => Vector3Int.zero
            };

            mb.transform.localPosition = new Vector3(tilePos.x + 0.5f, tilePos.y + 0.5f, 0f);

            bool isHorizontal = identity.PortSide is PortSide.North or PortSide.South;
            for (int i = -_portTileRadius; i <= _portTileRadius; i++)
            {
                Vector3Int target = new(
                    tilePos.x + (isHorizontal ? i : 0),
                    tilePos.y + (isHorizontal ? 0 : i), 0);

                if (!_tilemap.HasTile(target)) continue;
                
                _tilemap.SetTileFlags(target, TileFlags.None);
                _tilemap.SetColor(target, Color.clear);
            }
        }
    }
}