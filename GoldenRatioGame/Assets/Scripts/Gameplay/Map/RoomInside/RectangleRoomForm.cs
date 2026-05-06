using UnityEngine;
using UnityEngine.Tilemaps;

namespace IM.Map
{
    public class RectangleRoomForm : MonoBehaviour
    {
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private TileBase _floorTile;
        
        public Vector2Int Size { get; private set; }
        
        public void SetSize(Vector2Int size)
        {
            Size = size;
            GenerateRoom();
        }
        
        private void GenerateRoom()
        {
            if (!_tilemap || !_floorTile) return;

            _tilemap.ClearAllTiles();
            
            int startX = -Size.x / 2;
            int startY = -Size.y / 2;

            for (int x = 0; x < Size.x; x++)
            {
                for (int y = 0; y < Size.y; y++)
                {
                    Vector3Int pos = new Vector3Int(startX + x, startY + y, 0);
                    _tilemap.SetTile(pos, _floorTile);
                }
            }
        }
    }
}