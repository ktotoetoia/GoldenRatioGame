using UnityEngine;
using UnityEngine.Tilemaps;

namespace IM.Visuals
{
    [CreateAssetMenu(menuName = "2D/Tiles/Checker Tile")]
    public class CheckerTile : Tile
    {
        [field: SerializeField] public Sprite LightSprite { get; set; }
        [field: SerializeField] public Sprite DarkSprite { get; set; }

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            base.GetTileData(position, tilemap, ref tileData);

            bool isLight = (position.x + position.y) % 2 == 0;

            tileData.sprite = isLight ? LightSprite : DarkSprite;
        }
    }
}