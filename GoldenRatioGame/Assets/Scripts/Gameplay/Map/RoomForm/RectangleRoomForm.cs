using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IM.Map
{
    [DefaultExecutionOrder(-100)]
    [RequireComponent(typeof(GameObjectRoomMono))]
    public class RectangleRoomForm : MonoBehaviour, IRoomForm
    {
        [Header("Background Tilemap")]
        [SerializeField] private Tilemap _backgroundTilemap;
        [SerializeField] private TileBase _floorTile;
        [SerializeField] private TileBase _bgWallTile;
        [Tooltip("Tile 'n': Wall adjacent to a port")]
        [SerializeField] private TileBase _portWallTile;
        [Tooltip("Tile 'k': Wall where the room has an inner corner")]
        [SerializeField] private TileBase _cornerWallTile;

        [Header("Foreground Tilemap")]
        [SerializeField] private Tilemap _foregroundTilemap;
        [SerializeField] private TileBase _fgWallTile;
        [SerializeField] private Tile _innerWallTile;

        [SerializeField] private int _portTileRadius = 1;

        private GameObjectRoomMono _gameObjectRoomMono;
        private const int WallBorder = 1;

        private Tile _runtimeInnerWallTile;
        private readonly HashSet<Vector3Int> _portTilePositions = new();

        public IRoomShape RoomShape { get; private set; }

        private void Awake()
        {
            _gameObjectRoomMono = GetComponent<GameObjectRoomMono>();
            _gameObjectRoomMono.RoomPortAdded += SetUpRoomPort;
        }

        private void OnDestroy()
        {
            if (_gameObjectRoomMono != null)
                _gameObjectRoomMono.RoomPortAdded -= SetUpRoomPort;

            if (_runtimeInnerWallTile != null)
                Destroy(_runtimeInnerWallTile);
        }

        public void Apply(IRoomShape shape)
        {
            if (shape == null) return;

            RoomShape = shape;
            _portTilePositions.Clear();

            GenerateRoom(shape);

            if (_gameObjectRoomMono?.RoomPorts == null) return;
            foreach (IRoomPort port in _gameObjectRoomMono.RoomPorts)
                SetUpRoomPort(port);
        }

        private void GenerateRoom(IRoomShape shape)
        {
            if (!_backgroundTilemap || !_foregroundTilemap) return;

            _backgroundTilemap.ClearAllTiles();
            _foregroundTilemap.ClearAllTiles();

            var interior = BuildInteriorSet(shape);

            DrawBackground(interior);
            DrawForeground(interior);
        }

        private HashSet<Vector2Int> BuildInteriorSet(IRoomShape shape)
        {
            var set = new HashSet<Vector2Int>();
            foreach (Vector2Int offset in shape.Offsets)
            {
                var (startX, startY) = shape.Metrics.GetCellOrigin(offset);
                for (int x = startX; x < startX + shape.Metrics.CellW; x++)
                for (int y = startY; y < startY + shape.Metrics.CellH; y++)
                    set.Add(new Vector2Int(x, y));
            }
            return set;
        }

        private void DrawBackground(HashSet<Vector2Int> interior)
        {
            if (!_floorTile || !_bgWallTile) return;

            foreach (Vector2Int pos in interior)
                _backgroundTilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), _floorTile);

            var bgWallPositions = new HashSet<Vector2Int>();
            foreach (Vector2Int pos in interior)
            {
                var candidate = new Vector2Int(pos.x, pos.y + 1);
                if (!interior.Contains(candidate))
                    bgWallPositions.Add(candidate);
            }

            foreach (Vector2Int pos in bgWallPositions)
            {
                bool isCorner = interior.Contains(new Vector2Int(pos.x - 1, pos.y)) ||
                                interior.Contains(new Vector2Int(pos.x + 1, pos.y));

                TileBase tileToPlace = isCorner && _cornerWallTile != null ? _cornerWallTile : _bgWallTile;
                _backgroundTilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), tileToPlace);
            }
        }

        private void DrawForeground(HashSet<Vector2Int> interior)
        {
            if (!_fgWallTile) return;

            TileBase innerTile = GetOrCreateInnerWallTile();
            foreach (Vector2Int pos in interior)
                _foregroundTilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), innerTile);

            var borderPositions = new HashSet<Vector2Int>();
            foreach (Vector2Int pos in interior)
            {
                for (int dx = -1; dx <= 1; dx++)
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0) continue;
                    var candidate = new Vector2Int(pos.x + dx, pos.y + dy);
                    if (!interior.Contains(candidate))
                        borderPositions.Add(candidate);
                }
            }

            foreach (Vector2Int pos in borderPositions)
                _foregroundTilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), _fgWallTile);
        }

        private TileBase GetOrCreateInnerWallTile()
        {
            if (_innerWallTile != null) return _innerWallTile;
            if (_runtimeInnerWallTile != null) return _runtimeInnerWallTile;

            _runtimeInnerWallTile = ScriptableObject.CreateInstance<Tile>();
            _runtimeInnerWallTile.colliderType = Tile.ColliderType.None;
            _runtimeInnerWallTile.color = Color.clear;
            _runtimeInnerWallTile.flags = TileFlags.None;

            if (_fgWallTile is Tile sourceTile)
                _runtimeInnerWallTile.sprite = sourceTile.sprite;

            return _runtimeInnerWallTile;
        }

        private void SetUpRoomPort(IRoomPort roomPort)
        {
            if (roomPort is not MonoBehaviour mb || RoomShape == null || !RoomShape.Metrics.IsValid) return;
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
                    tilePos.y + (isHorizontal ? 0 : i),
                    0);

                ClearTile(_backgroundTilemap, target);
                ClearTile(_foregroundTilemap, target);
                _portTilePositions.Add(target);
            }

            if (_portWallTile != null)
            {
                Vector3Int prevTile = new(
                    tilePos.x + (isHorizontal ? -_portTileRadius - 1 : 0),
                    tilePos.y + (isHorizontal ? 0 : -_portTileRadius - 1), 0);

                Vector3Int nextTile = new(
                    tilePos.x + (isHorizontal ? _portTileRadius + 1 : 0),
                    tilePos.y + (isHorizontal ? 0 : _portTileRadius + 1), 0);

                if (IsBackgroundWall(prevTile)) _backgroundTilemap.SetTile(prevTile, _portWallTile);
                if (IsBackgroundWall(nextTile)) _backgroundTilemap.SetTile(nextTile, _portWallTile);
            }
        }

        private static void ClearTile(Tilemap tilemap, Vector3Int pos)
        {
            if (!tilemap.HasTile(pos)) return;
            tilemap.SetTileFlags(pos, TileFlags.None);
            tilemap.SetColor(pos, Color.clear);
            tilemap.SetTile(pos, null);
        }

        private bool IsBackgroundWall(Vector3Int pos)
        {
            TileBase tile = _backgroundTilemap.GetTile(pos);
            return tile != null && tile != _floorTile;
        }
    }
}