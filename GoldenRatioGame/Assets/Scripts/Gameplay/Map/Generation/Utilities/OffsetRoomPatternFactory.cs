using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IM.Map
{
    public class OffsetRoomPatternFactory
    {
        public IRoomPattern Create(Vector2Int cellSize, IEnumerable<Vector2Int> inputOffsets)
        {
            if (inputOffsets == null) throw new ArgumentNullException(nameof(inputOffsets));

            HashSet<Vector2Int> offsets = inputOffsets.ToHashSet();
            if (offsets.Count == 0) throw new ArgumentException("Offsets collection cannot be empty.", nameof(inputOffsets));

            Dictionary<Vector2Int, IEnumerable<IPortDefinition>> optionalPorts = new();
            int globalPortIndex = 0;

            foreach (Vector2Int offset in offsets)
            {
                List<IPortDefinition> ports = new();

                if (!offsets.Contains(offset + Vector2Int.left))
                    ports.Add(new PortDefinition(PortSide.West, globalPortIndex++));

                if (!offsets.Contains(offset + Vector2Int.right))
                    ports.Add(new PortDefinition(PortSide.East, globalPortIndex++));

                if (!offsets.Contains(offset + Vector2Int.down))
                    ports.Add(new PortDefinition(PortSide.South, globalPortIndex++));

                if (!offsets.Contains(offset + Vector2Int.up))
                    ports.Add(new PortDefinition(PortSide.North, globalPortIndex++));

                if (ports.Count > 0)
                {
                    optionalPorts[offset] = ports;
                }
            }

            return new RoomPattern(
                new RoomShape(new Rect(Vector2.zero, cellSize), offsets),
                new Dictionary<Vector2Int, IEnumerable<IPortDefinition>>(), optionalPorts
            );
        }
    }
}