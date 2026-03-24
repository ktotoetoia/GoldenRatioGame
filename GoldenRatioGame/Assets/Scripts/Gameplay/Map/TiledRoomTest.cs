using UnityEngine;
using UnityEngine.Tilemaps;

namespace IM.Map
{
    public class TiledRoomTest : MonoBehaviour
    {
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private TileBase _fillTile;
        [SerializeField] private TileBase _borderTile;
        [SerializeField] private BoundsInt _bounds  = new (Vector3Int.one * -5, Vector3Int.one * 10);
        private IRoom _room;
        
        private void Awake()
        {
            _room = new TiledRoomFactory(_tilemap,_fillTile, _borderTile).Create(_bounds);
        }
    }
}