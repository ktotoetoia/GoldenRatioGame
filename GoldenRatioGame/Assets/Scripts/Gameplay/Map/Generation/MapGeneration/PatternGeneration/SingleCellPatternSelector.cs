using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace IM.Map.Grid
{
    public class SingleCellPatternSelector : IPatternSelector
    {
        public void SelectMatchingRoomPatterns(IGrid<ICellInfo> grid, int seed)
        {
            Random random = new Random(seed);
            
            foreach (Vector2Int pos in grid.OccupiedPositions())
            {
                ICellInfo cell = grid[pos];
                cell.Pattern = SelectPattern(grid, cell, pos, random);
            }
        }

        private IRoomPattern SelectPattern(IGrid<ICellInfo> grid, ICellInfo cell, Vector2Int pos, Random random)
        {
            HashSet<PortSide> occupiedSides = GetOccupiedNeighborSides(grid, pos);

            List<IRoomPattern> available = cell.Factory.GetRoomPatterns()
                .Where(pattern =>pattern.Shape.Offsets.Count == 1 && IsCompatible(pattern, occupiedSides))
                .ToList();

            return available[random.Next(available.Count)];
        }

        private HashSet<PortSide> GetOccupiedNeighborSides(IGrid<ICellInfo> grid, Vector2Int pos)
        {
            HashSet<PortSide> sides = new();

            foreach (Vector2Int dir in PortSideUtility.Directions)
                if (grid.IsOccupied(pos + dir))
                    sides.Add(PortSideUtility.FromDirection(dir));

            return sides;
        }

        private bool IsCompatible(IRoomPattern pattern, HashSet<PortSide> occupiedSides)
        {
            HashSet<PortSide> patternSides = pattern.RequiredPortDefinitions
                .Concat(pattern.OptionalPortDefinitions)
                .SelectMany(p => p.Value.Select(x => x.Side))
                .ToHashSet();

            return occupiedSides.All(side => patternSides.Contains(side));
        }
    }
}