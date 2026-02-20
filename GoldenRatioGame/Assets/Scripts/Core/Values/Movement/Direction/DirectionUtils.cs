using UnityEngine;

namespace IM.Values
{
    public static class DirectionUtils
    {
        public static Direction GetEnumDirection(Vector2 vectorDirection)
        {
            Direction dir = Direction.None;

            if (vectorDirection.x > 0f) dir |= Direction.Right;
            if (vectorDirection.x < 0f) dir |= Direction.Left;
            if (vectorDirection.y > 0f) dir |= Direction.Up;
            if (vectorDirection.y < 0f) dir |= Direction.Down;

            return dir;
        }
    }
}