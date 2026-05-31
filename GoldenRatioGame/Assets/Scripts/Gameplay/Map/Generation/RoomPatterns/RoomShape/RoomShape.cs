using System.Collections.Generic;
using UnityEngine;

namespace IM.Map
{
    public class RoomShape : IRoomShape
    {
        public Rect CellRect { get; }
        public HashSet<Vector2Int> Offsets { get; }
        public ShapeMetrics Metrics { get; }

        public RoomShape(Rect cellRect) : this(cellRect, new HashSet<Vector2Int>() {Vector2Int.zero})
        {
            
        }
        
        public RoomShape(Rect cellRect, HashSet<Vector2Int> offsets)
        {
            CellRect = cellRect;
            Offsets = offsets;
            Metrics = ShapeMetrics.From(this);
        }
    }
}