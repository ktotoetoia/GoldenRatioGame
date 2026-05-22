using System.Collections.Generic;
using System.Linq;
using System.Text;
using IM.Map.Grid;
using UnityEngine;

namespace IM.Map
{
    [CreateAssetMenu(menuName = "Map/Generation Steps/Grid Debug Printing Step")]
    public class GridDebugPrintingStep : MapGenerationStep
    {
        public override void Execute(MapGenerationContext context)
        {
            var grid = context.Grid;
            
            if (grid == null) Debug.LogWarning("grid is null");
            
            var occupied = grid.OccupiedPositions().ToList();
            if (occupied.Count == 0)
            {
                Debug.Log("--- Grid is Empty ---");
                return;
            }

            int minX = occupied.Min(p => p.x);
            int maxX = occupied.Max(p => p.x);
            int minY = occupied.Min(p => p.y);
            int maxY = occupied.Max(p => p.y);

            Dictionary<IRoomPattern, int> patternIds = new Dictionary<IRoomPattern, int>();
            int nextId = 1;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"--- Generated Map Grid Layout ({maxX - minX + 1}x{maxY - minY + 1}) ---");

            for (int y = maxY; y >= minY; y--)
            {
                StringBuilder rowBuilder = new StringBuilder();

                for (int x = minX; x <= maxX; x++)
                {
                    Vector2Int pos = new Vector2Int(x, y);

                    if (!grid.IsOccupied(pos))
                    {
                        rowBuilder.Append("[     ]");
                        continue;
                    }

                    ICellInfo cell = grid[pos];
                    if (cell.Pattern == null)
                    {
                        rowBuilder.Append("[Unpatt]");
                        continue;
                    }

                    if (!patternIds.TryGetValue(cell.Pattern, out int id))
                    {
                        id = nextId++;
                        patternIds[cell.Pattern] = id;
                    }

                    int width = Mathf.RoundToInt(cell.Pattern.Shape.CellRect.width / 17f);
                    int height = Mathf.RoundToInt(cell.Pattern.Shape.CellRect.height / 9f);
                    if (width <= 0) width = 1;
                    if (height <= 0) height = 1;

                    string typeLabel = $"{width}x{height}";

                    rowBuilder.Append($"[R{cell.RoomInstanceId}]");
                }

                sb.AppendLine(rowBuilder.ToString());
            }

            sb.AppendLine("-----------------------------------------------------");
            Debug.Log(sb.ToString());
        }
    }
}