using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IM.Map
{
    public class ForegroundRoomLayer : MonoBehaviour, IRoomFormLayer
    {
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private TileBase _wallTile;
        private Tile _runtimeInnerWallTile;
 
        private void OnDestroy()
        {
            if (_runtimeInnerWallTile != null)
                Destroy(_runtimeInnerWallTile);
        }
 
        public void Apply(IRoomShape shape)
        {
            if (!_tilemap || !_wallTile) return;
 
            _tilemap.ClearAllTiles();
            Draw(BuildInteriorSet(shape));
        }
 
        public void SetupPort(PortTileInfo portInfo)
        {
            foreach (Vector3Int pos in portInfo.ClearedTiles())
                ClearTile(pos);
        }
 
        private void Draw(HashSet<Vector2Int> interior)
        {
            TileBase innerTile = GetOrCreateInnerWallTile();
 
            foreach (Vector2Int pos in interior)
                _tilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), innerTile);
 
            foreach (Vector2Int pos in interior)
            {
                for (int dx = -1; dx <= 1; dx++)
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0) continue;
 
                    var candidate = new Vector2Int(pos.x + dx, pos.y + dy);
                    if (!interior.Contains(candidate))
                        _tilemap.SetTile(new Vector3Int(candidate.x, candidate.y, 0), _wallTile);
                }
            }
        }
 
        private void ClearTile(Vector3Int pos)
        {
            if (!_tilemap.HasTile(pos)) return;
            _tilemap.SetTileFlags(pos, TileFlags.None);
            _tilemap.SetColor(pos, Color.clear);
            _tilemap.SetTile(pos, null);
        }

        private TileBase GetOrCreateInnerWallTile()
        {
            if (_runtimeInnerWallTile) return _runtimeInnerWallTile;
 
            _runtimeInnerWallTile = ScriptableObject.CreateInstance<Tile>();
            _runtimeInnerWallTile.colliderType = Tile.ColliderType.None;
            _runtimeInnerWallTile.color = Color.clear;
            _runtimeInnerWallTile.flags = TileFlags.None;
 
            if (_wallTile is Tile sourceTile)
                _runtimeInnerWallTile.sprite = sourceTile.sprite;
 
            return _runtimeInnerWallTile;
        }
 
        private static HashSet<Vector2Int> BuildInteriorSet(IRoomShape shape)
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
    }
}