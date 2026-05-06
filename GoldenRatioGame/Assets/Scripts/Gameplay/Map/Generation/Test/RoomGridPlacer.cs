using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace IM.Map.Grid
{
    public class RoomGridPlacer
    {
        private readonly Random _random;

        public RoomGridPlacer(Random random) => _random = random;

        public void Place(RoomGrid grid, IRoomFactory factory, Vector2Int position)
        {
            grid[position] = new CellInfo(PickPattern(factory), factory);
        }

        public bool PlaceClose(RoomGrid grid, IRoomFactory factory)
        {
            List<Vector2Int> available = GetAvailablePositions(grid);
            if (available.Count == 0) return false;

            available = available.OrderBy(_ => _random.Next()).ToList();
            List<IRoomPattern> patterns = factory.GetRoomPatterns()
                .OrderBy(_ => _random.Next()).ToList();

            foreach (Vector2Int pos in available)
            {
                foreach (IRoomPattern pattern in patterns)
                {
                    if (!HasPortTowardAnyNeighbor(grid, pattern, pos)) continue;
                    grid[pos] = new CellInfo(pattern, factory);
                    return true;
                }
            }

            return false;
        }

        private List<Vector2Int> GetAvailablePositions(RoomGrid grid)
        {
            List<Vector2Int> positions = new();

            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    Vector2Int pos = new(x, y);
                    if (grid.IsOccupied(pos)) continue;
                    if (HasOccupiedNeighborPointingHere(grid, pos))
                        positions.Add(pos);
                }
            }

            return positions;
        }

        private bool HasOccupiedNeighborPointingHere(RoomGrid grid, Vector2Int pos)
        {
            foreach (Vector2Int dir in PortSideUtility.Directions)
            {
                Vector2Int neighborPos = pos + dir;
                if (!grid.IsOccupied(neighborPos)) continue;

                PortSide sidePointingBack = PortSideUtility.FromDirection(-dir);
                if (HasPort(grid[neighborPos].Pattern, sidePointingBack)) return true;
            }

            return false;
        }

        private bool HasPortTowardAnyNeighbor(RoomGrid grid, IRoomPattern pattern, Vector2Int pos)
        {
            foreach (Vector2Int dir in PortSideUtility.Directions)
            {
                Vector2Int neighborPos = pos + dir;
                if (!grid.IsOccupied(neighborPos)) continue;

                PortSide sideTowardNeighbor = PortSideUtility.FromDirection(dir);
                if (HasPort(pattern, sideTowardNeighbor)) return true;
            }

            return false;
        }

        private bool HasPort(IRoomPattern pattern, PortSide side) =>
            pattern.RequiredPortDefinitions
                .Concat(pattern.OptionalPortDefinitions)
                .Any(p => p.Side == side);

        private IRoomPattern PickPattern(IRoomFactory factory)
        {
            List<IRoomPattern> patterns = factory.GetRoomPatterns().ToList();
            return patterns[_random.Next(patterns.Count)];
        }
    }
}