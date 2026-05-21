using System;
using System.Collections.Generic;
using UnityEngine;

namespace IM.Map
{
    public class RoomShape : IRoomShape
    {
        public Rect CellRect { get; }
        public Rect RoomRect { get; }
        public HashSet<Vector2Int> Offsets { get; }

        public RoomShape(Rect cellRect, Rect roomRect) : this(cellRect,roomRect, new HashSet<Vector2Int>() {Vector2Int.zero})
        {
            
        }
        
        public RoomShape(Rect cellRect, Rect roomRect, HashSet<Vector2Int> offsets)
        {
            if (roomRect.xMin < cellRect.xMin ||
                roomRect.yMin < cellRect.yMin ||
                roomRect.xMax > cellRect.xMax ||
                roomRect.yMax > cellRect.yMax)
            {
                throw new ArgumentException(
                    $"RoomRect {roomRect} must be contained within ReservedRect {cellRect}");
            }

            CellRect = cellRect;
            RoomRect = roomRect;
            Offsets = offsets;
        }
    }
}