using UnityEngine;
using UnityEngine.Tilemaps;

namespace IM.Map
{
    public class TiledRoomFactory : ITiledRoomFactory
    {
        public Tilemap Tilemap { get; set; }
        public TileBase BorderTile { get; set; }
        public TileBase FillTile { get; set; }
        
        public TiledRoomFactory(Tilemap tilemap, TileBase borderTile, TileBase fillTile)
        {
            BorderTile = borderTile;
            FillTile = fillTile;
            Tilemap = tilemap;
        }

        public IRoom Create(BoundsInt bounds)
        {
            TileBase[,] layout = new TileBase[bounds.size.x, bounds.size.y];

            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    Tilemap.SetTile(new Vector3Int(x, y), FillTile);
                }
            }
            
            return new TiledRoom(layout,Tilemap);
        }
    }
}