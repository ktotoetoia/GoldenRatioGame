using System.Collections.Generic;
using UnityEngine;

namespace IM.Map
{
    /// <summary>
    /// Inclusive bottom-left/top-right cell range a prop occupies in room-local grid space.
    /// </summary>
    public readonly struct PropPlacementInfo
    {
        public Vector2Int StartCell { get; }
        public Vector2Int EndCell { get; }
        
        public Vector2 Center => new Vector2(EndCell.x + StartCell.x, EndCell.y + StartCell.y)/2;

        public PropPlacementInfo(Vector2Int startCell, Vector2Int endCell)
        {
            StartCell = startCell;
            EndCell = endCell;
        }

        public Vector2Int Size => EndCell - StartCell + Vector2Int.one;

        public static PropPlacementInfo FromOrigin(Vector2Int bottomLeftCell, Vector2Int size)
        {
            return new PropPlacementInfo(bottomLeftCell, bottomLeftCell + size - Vector2Int.one);
        }

        public bool Contains(Vector2Int cell)
        {
            return cell.x >= StartCell.x && cell.x <= EndCell.x &&
                   cell.y >= StartCell.y && cell.y <= EndCell.y;
        }

        public IEnumerable<Vector2Int> EnumerateCells()
        {
            for (int x = StartCell.x; x <= EndCell.x; x++)
            {
                for (int y = StartCell.y; y <= EndCell.y; y++)
                {
                    yield return new Vector2Int(x, y);
                }
            }
        }
    }
}