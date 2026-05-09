using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IM.Map.Grid
{
    public class Grid<T> : IGrid<T>
    {
        private readonly T[,] _cells;
        public int Width => _cells.GetLength(0);
        public int Height => _cells.GetLength(1);

        public Grid(int width, int height) => _cells = new T[width, height];

        public T this[int x, int y]
        {
            get => _cells[x, y];
            set => _cells[x, y] = value;
        }

        public T this[Vector2Int pos]
        {
            get => _cells[pos.x, pos.y];
            set => _cells[pos.x, pos.y] = value;
        }

        public bool InBounds(Vector2Int pos) =>
            pos is { x: >= 0, y: >= 0 } && pos.x < Width && pos.y < Height;

        public bool IsOccupied(Vector2Int pos) => InBounds(pos) && _cells[pos.x, pos.y] != null;

        public IEnumerable<Vector2Int> OccupiedPositions()
        {
            for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
                if (_cells[x, y] != null) 
                    yield return new Vector2Int(x, y);
        }

        public IEnumerator<T> GetEnumerator() => _cells.Cast<T>().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}