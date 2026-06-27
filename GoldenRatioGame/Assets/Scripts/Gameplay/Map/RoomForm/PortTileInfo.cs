using System.Collections.Generic;
using UnityEngine;

namespace IM.Map
{
    public readonly struct PortTileInfo
    {
        public readonly Vector3Int Center;
        public readonly bool IsHorizontal;
        public readonly int Radius;
 
        public PortTileInfo(Vector3Int center, bool isHorizontal, int radius)
        {
            Center       = center;
            IsHorizontal = isHorizontal;
            Radius       = radius;
        }
 
        public IEnumerable<Vector3Int> ClearedTiles()
        {
            for (int i = -Radius; i <= Radius; i++)
                yield return new Vector3Int(
                    Center.x + (IsHorizontal ? i : 0),
                    Center.y + (IsHorizontal ? 0 : i),
                    0);
        }
 
        public Vector3Int AdjacentTile(int direction)
        {
            int offset = direction > 0 ? Radius + 1 : -Radius - 1;
            return new Vector3Int(
                Center.x + (IsHorizontal ? offset : 0),
                Center.y + (IsHorizontal ? 0 : offset),
                0);
        }
    }
}