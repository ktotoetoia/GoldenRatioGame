using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IM.Map.Grid
{
    public class RoomGridSelector
    {
        public void SelectAll(RoomGrid grid)
        {
            foreach (Vector2Int pos in grid.OccupiedPositions())
            {
                CellInfo cell = grid[pos];

                List<IPortDefinition> validOptionals = cell.Pattern.OptionalPortDefinitions
                    .Where(port => grid.IsOccupied(pos + PortSideUtility.ToDirection(port.Side)))
                    .ToList();

                cell.Select(validOptionals);
            }
        }
    }
}