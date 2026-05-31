using System.Collections.Generic;
using UnityEngine;

namespace IM.Map
{
    public interface IRoomShape
    {
        Rect CellRect { get; }
        HashSet<Vector2Int> Offsets { get; }
        ShapeMetrics Metrics { get; }
    }
}