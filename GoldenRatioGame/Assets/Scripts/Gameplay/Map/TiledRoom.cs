using UnityEngine.Tilemaps;

namespace IM.Map
{
    public class TiledRoom : IRoom
    {
        public TileBase[,] Layout { get; }
        public Tilemap Tilemap { get; }
        
        public TiledRoom(TileBase[,] layout, Tilemap tilemap)
        {
            Layout = layout;
            Tilemap = tilemap;
        }
        
        public void Enter()
        {
            
        }

        public void Exit()
        {
            
        }
    }
}