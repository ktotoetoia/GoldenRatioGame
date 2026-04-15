using System;
using System.Collections.Generic;
using IM.SaveSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Tilemaps;

namespace IM
{
    public class TilemapAddressableSerializer : ComponentSerializer<Tilemap>
    {
        public override object CaptureState(Tilemap component)
        {
            var state = new TilemapState();
            foreach (var pos in component.cellBounds.allPositionsWithin)
            {
                if (component.HasTile(pos))
                {
                    TileBase tile = component.GetTile(pos);
                    
                    state.Tiles.Add(new TileData { 
                        Position = pos, 
                        TileAssetName = tile.name 
                    });
                }
            }
            return state;
        }

        public override void RestoreState(Tilemap component, object state, Func<string, GameObject> resolveDependency)
        {
            if (state is not TilemapState tilemapState) return;

            component.ClearAllTiles();

            foreach (var tileData in tilemapState.Tiles)
            {
                var op = Addressables.LoadAssetAsync<TileBase>(tileData.TileAssetName);
                TileBase tile = op.WaitForCompletion();

                if (tile != null)
                {
                    component.SetTile(tileData.Position, tile);
                }
            }
        }
        
        [Serializable]
        public class TilemapState
        {
            public List<TileData> Tiles = new();
        }

        [Serializable]
        public struct TileData
        {
            public Vector3Int Position;
            public string TileAssetName;
        }
    }
}