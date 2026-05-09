using System;
using UnityEngine;

namespace IM.Map.Grid
{
    public static class PortSideUtility
    {
        public static PortSide FromDirection(Vector2Int dir) => dir switch
        {
            { x: 0, y: 1 }  => PortSide.North,
            { x: 0, y: -1 } => PortSide.South,
            { x: 1, y: 0 }  => PortSide.East,
            { x: -1, y: 0 } => PortSide.West,
            _ => throw new ArgumentException($"Invalid direction {dir}")
        };

        public static Vector2Int ToDirection(PortSide side) => side switch
        {
            PortSide.North => Vector2Int.up,
            PortSide.South => Vector2Int.down,
            PortSide.East  => Vector2Int.right,
            PortSide.West  => Vector2Int.left,
            _ => throw new ArgumentException($"Invalid side {side}")
        };

        public static PortSide Opposite(PortSide side) => side switch
        {
            PortSide.North => PortSide.South,
            PortSide.South => PortSide.North,
            PortSide.East  => PortSide.West,
            PortSide.West  => PortSide.East,
            _ => throw new ArgumentException($"Invalid side {side}")
        };

        public static readonly Vector2Int[] Directions =
        {
            Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
        };
    }
}