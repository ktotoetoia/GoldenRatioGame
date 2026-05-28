using UnityEngine;
using UnityEngine.Tilemaps;

namespace IM.Map
{
    [DefaultExecutionOrder(-100)]
    [RequireComponent(typeof(GameObjectRoomMono))]
    public class RectangleRoomForm : MonoBehaviour, IRoomForm
    {
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private TileBase _floorTile;
        [SerializeField] private TileBase _wallTile;
        [SerializeField] private int _portTileRadius = 1;
        private GameObjectRoomMono _gameObjectRoomMono;
        private ShapeMetrics _metrics;
        private const int WallBorder = 1;
        
        public IRoomShape RoomShape { get; private set; }

        private void Awake()
        {
            _gameObjectRoomMono = GetComponent<GameObjectRoomMono>();
            _gameObjectRoomMono.RoomPortAdded += SetUpRoomPort;
        }

        private void OnDestroy()
        {
            if (_gameObjectRoomMono != null) _gameObjectRoomMono.RoomPortAdded -= SetUpRoomPort;
        }

        public void Apply(IRoomShape shape)
        {
            if (shape == null) return;

            RoomShape = shape;
            _metrics = ShapeMetrics.From(shape);

            GenerateRoom(shape);

            if (_gameObjectRoomMono?.RoomPorts == null) return;
            
            foreach (IRoomPort port in _gameObjectRoomMono.RoomPorts) SetUpRoomPort(port);
        }

        private void GenerateRoom(IRoomShape shape)
        {
            if (!_tilemap || !_floorTile || !_wallTile) return;
            _tilemap.ClearAllTiles();

            foreach (Vector2Int offset in shape.Offsets)
            {
                var (startX, startY) = _metrics.GetCellOrigin(offset);
                for (int x = -WallBorder; x < _metrics.CellW + WallBorder; x++)
                for (int y = -WallBorder; y < _metrics.CellH + WallBorder; y++)
                    _tilemap.SetTile(new Vector3Int(startX + x, startY + y, 0), _wallTile);
            }

            foreach (Vector2Int offset in shape.Offsets)
            {
                var (startX, startY) = _metrics.GetCellOrigin(offset);
                for (int x = 0; x < _metrics.CellW; x++)
                for (int y = 0; y < _metrics.CellH; y++)
                    _tilemap.SetTile(new Vector3Int(startX + x, startY + y, 0), _floorTile);
            }
        }

        private void SetUpRoomPort(IRoomPort roomPort)
        {
            if (roomPort is not MonoBehaviour mb || !_metrics.IsValid) return;
            ResolvePortPosition(mb, roomPort.PortIdentity);
        }

        private void ResolvePortPosition(MonoBehaviour mb, IPortIdentity identity)
        {
            var (cellStartX, cellStartY) = _metrics.GetCellOrigin(identity.CellOffset);

            int wallLeft   = cellStartX - WallBorder;
            int wallRight  = cellStartX + _metrics.CellW - 1 + WallBorder;
            int wallBottom = cellStartY - WallBorder;
            int wallTop    = cellStartY + _metrics.CellH - 1 + WallBorder;

            float norm = identity.NormalizedPosition;

            Vector3Int tilePos = identity.PortSide switch
            {
                PortSide.North => new Vector3Int(Mathf.RoundToInt(Mathf.Lerp(wallLeft, wallRight, norm)), wallTop, 0),
                PortSide.South => new Vector3Int(Mathf.RoundToInt(Mathf.Lerp(wallLeft, wallRight, norm)), wallBottom, 0),
                PortSide.East  => new Vector3Int(wallRight, Mathf.RoundToInt(Mathf.Lerp(wallBottom, wallTop, norm)), 0),
                PortSide.West  => new Vector3Int(wallLeft,  Mathf.RoundToInt(Mathf.Lerp(wallBottom, wallTop, norm)), 0),
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

        public readonly struct ShapeMetrics
        {
            public readonly int CellW, CellH;
            public readonly int TotalW, TotalH;
            public readonly bool IsValid;
            private readonly int _minOffX, _minOffY;
            private readonly int _roomOriginX, _roomOriginY;

            private ShapeMetrics(IRoomShape shape)
            {
                CellW = (int)shape.CellRect.width;
                CellH = (int)shape.CellRect.height;

                int minX = int.MaxValue, minY = int.MaxValue;
                int maxX = int.MinValue, maxY = int.MinValue;
                bool hasOffsets = false;

                foreach (Vector2Int offset in shape.Offsets)
                {
                    if (offset.x < minX) minX = offset.x;
                    if (offset.x > maxX) maxX = offset.x;
                    if (offset.y < minY) minY = offset.y;
                    if (offset.y > maxY) maxY = offset.y;
                    hasOffsets = true;
                }

                if (!hasOffsets)
                {
                    _minOffX = _minOffY = TotalW = TotalH = _roomOriginX = _roomOriginY = 0;
                    IsValid = false;
                    return;
                }

                _minOffX = minX;
                _minOffY = minY;
                
                TotalW = (maxX - minX + 1) * CellW;
                TotalH = (maxY - minY + 1) * CellH;
                
                _roomOriginX = -TotalW / 2;
                _roomOriginY = -TotalH / 2;
                IsValid = true;
            }

            public static ShapeMetrics From(IRoomShape shape) => new(shape);

            public (int startX, int startY) GetCellOrigin(Vector2Int cellOffset) => (
                _roomOriginX + (cellOffset.x - _minOffX) * CellW,
                _roomOriginY + (cellOffset.y - _minOffY) * CellH);
        }
    }
}