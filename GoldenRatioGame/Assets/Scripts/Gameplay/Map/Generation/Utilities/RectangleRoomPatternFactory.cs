using System.Collections.Generic;
using UnityEngine;

namespace IM.Map
{
    public class RectangleRoomPatternFactory
    {
        public IRoomPattern Create(Vector2Int roomSize, Vector2Int cells)
        {
            if (cells.x <= 0 || cells.y <= 0)
                throw new System.ArgumentOutOfRangeException(nameof(cells), "Cells must be positive.");

            Vector2Int totalSize = Vector2Int.Scale(roomSize, cells);

            HashSet<Vector2Int> offsets = new();
            Dictionary<Vector2Int, IEnumerable<IPortDefinition>> optionalPorts = new();

            for (int x = 0; x < cells.x; x++)
            {
                for (int y = 0; y < cells.y; y++)
                {
                    Vector2Int offset = new Vector2Int(x, y);
                    offsets.Add(offset);

                    List<IPortDefinition> ports = new();

                    if (x == 0)
                        ports.Add(new PortDefinition(PortSide.West));

                    if (x == cells.x - 1)
                        ports.Add(new PortDefinition(PortSide.East));

                    if (y == 0)
                        ports.Add(new PortDefinition(PortSide.South));

                    if (y == cells.y - 1)
                        ports.Add(new PortDefinition(PortSide.North));

                    if (ports.Count > 0)
                        optionalPorts[offset] = ports;
                }
            }

            return new RoomPattern(
                new RoomShape(
                    new Rect(Vector2.zero, totalSize),
                    new Rect(Vector2.zero, totalSize),
                    offsets),
                new Dictionary<Vector2Int, IEnumerable<IPortDefinition>>(),
                optionalPorts);
        }
    }
}