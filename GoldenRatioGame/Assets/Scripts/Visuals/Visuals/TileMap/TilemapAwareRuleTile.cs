using UnityEngine;
using UnityEngine.Tilemaps;

namespace IM.Visuals
{
    [CreateAssetMenu(menuName = "2D/Tiles/Tilemap Aware Random Rule Tile")]
    public class TilemapAwareRuleTile : RuleTile
    {
        [Header("Custom Random Settings")]
        [SerializeField] private Sprite[] _customRandomSprites;

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            base.GetTileData(position, tilemap, ref tileData);
            
            if (_customRandomSprites is { Length: > 0 })
            {
                int hash = (position.x * 73856093) ^ (position.y * 19349663) ^ (position.z * 83492791);

                TilemapSeed tmSeed = tilemap.GetComponent<TilemapSeed>();

                if (tmSeed != null)
                {
                    hash ^= (tmSeed.Seed * 31);
                }
                
                System.Random prng = new System.Random(hash);

                int randomIndex = prng.Next(0, _customRandomSprites.Length);
                tileData.sprite = _customRandomSprites[randomIndex];
            }
        }
    }
}