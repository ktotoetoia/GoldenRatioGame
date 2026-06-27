using System.Collections.Generic;
using System.Linq;
using IM.Values;

using UnityEngine;
using UnityEngine.Tilemaps;

namespace IM.Map
{
    public class BackgroundRoomLayer : MonoBehaviour, IRoomFormLayer
    {
        [SerializeField] private Tilemap  _tilemap;
        [SerializeField] private WeightedRandom<TileBase> _floorTile;
        [SerializeField] private WeightedRandom<TileBase> _wallTiles;
        [SerializeField] private WeightedRandom<TileBase> _portWallTiles;
        [SerializeField] private WeightedRandom<TileBase> _cornerWallTiles;
 
        public void Apply(IRoomShape shape)
        {
            if (!_tilemap || _floorTile == null || _wallTiles == null) return;

            _tilemap.ClearAllTiles();
            Draw(BuildInteriorSet(shape));
        }
 
        public void SetupPort(PortTileInfo portInfo)
        {
            foreach (Vector3Int pos in portInfo.ClearedTiles()) ClearTile(pos);
            
            if (_portWallTiles == null) return;
 
            Vector3Int prev = portInfo.AdjacentTile(-1);
            Vector3Int next = portInfo.AdjacentTile(+1);
 
            if (IsWall(prev)) _tilemap.SetTile(prev, _portWallTiles.Next());
            if (IsWall(next)) _tilemap.SetTile(next, _portWallTiles.Next());
        }
 
        private void Draw(HashSet<Vector2Int> interior)
        {
            foreach (Vector2Int pos in interior) _tilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), _floorTile.Next());
 
            foreach (Vector2Int pos in interior)
            {
                var wallPos = new Vector2Int(pos.x, pos.y + 1);
                if (interior.Contains(wallPos)) continue;
 
                bool isCorner = interior.Contains(new Vector2Int(wallPos.x - 1, wallPos.y)) ||
                                interior.Contains(new Vector2Int(wallPos.x + 1, wallPos.y));
 
                TileBase tile = isCorner && _cornerWallTiles != null ? _cornerWallTiles.Next() : _wallTiles.Next();
                _tilemap.SetTile(new Vector3Int(wallPos.x, wallPos.y, 0), tile);
            }
        }
 
        private void ClearTile(Vector3Int pos)
        {
            if (!_tilemap.HasTile(pos)) return;
            _tilemap.SetTileFlags(pos, TileFlags.None);
            _tilemap.SetColor(pos, Color.clear);
            _tilemap.SetTile(pos, null);
        }
 
        private bool IsWall(Vector3Int pos)
        {
            TileBase tile = _tilemap.GetTile(pos);
            
            return tile && _floorTile.Entries.All(x => x.item != tile);
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