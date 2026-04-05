using UnityEngine;
using UnityEngine.Tilemaps;

namespace IM.Visuals
{
    public class RoomGenerator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private TileBase floorTile;

        [Header("Settings")]
        [SerializeField] private Vector2Int minBounds;
        [SerializeField] private Vector2Int maxBounds;

        private void Awake()
        {
            GenerateRoom();
        }
        
        public void GenerateRoom()
        {
            if (!tilemap || !floorTile) return;

            tilemap.ClearAllTiles();
            
            int width = Random.Range(minBounds.x, maxBounds.x + 1);
            int height = Random.Range(minBounds.y, maxBounds.y + 1);

            int startX = -width / 2;
            int startY = -height / 2;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector3Int pos = new Vector3Int(startX + x, startY + y, 0);
                    tilemap.SetTile(pos, floorTile);
                }
            }
        }
    }
}