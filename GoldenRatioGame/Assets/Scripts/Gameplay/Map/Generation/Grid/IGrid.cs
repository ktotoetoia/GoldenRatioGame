using System.Collections.Generic;
using UnityEngine;

namespace IM.Map.Grid
{
    public interface IGrid<T> : IEnumerable<T>
    {
        int Width { get; }
        int Height { get; }
        T this[int x, int y] { get; set; }
        T this[Vector2Int pos] { get; set; }
        bool InBounds(Vector2Int pos);
        bool IsOccupied(Vector2Int pos);
        IEnumerable<Vector2Int> OccupiedPositions();
    }
}