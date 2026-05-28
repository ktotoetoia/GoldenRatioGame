using UnityEngine;

namespace IM.Map
{
    public interface IPortIdentity
    {
        int Index { get; }
        float NormalizedPosition { get; }
        Vector2Int CellOffset { get; }
        PortSide PortSide { get; }
    }
}