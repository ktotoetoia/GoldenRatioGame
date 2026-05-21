using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IM.Map.Grid
{
    public static class GridUtility
    {
        public static List<Vector2Int> GetNearbyUnoccupiedPosition<T>(IGrid<T> grid)
        {
            List<Vector2Int> positions = new();

            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    Vector2Int targetPosition = new(x, y);
                    
                    if (grid.IsOccupied(targetPosition)) continue;
                    
                    foreach (Vector2Int dir in PortSideUtility.Directions)
                    {
                        Vector2Int neighborPos = targetPosition + dir;
                        
                        if (positions.Any(pos => pos.Equals(neighborPos)) ||!grid.IsOccupied(neighborPos)) continue;

                        positions.Add(targetPosition);
                    }
                }
            }

            return positions;
        }
    }
}