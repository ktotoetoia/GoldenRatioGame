using UnityEngine;

namespace IM.Map
{
    public class PortIdentity : IPortIdentity
    {
        public int Index { get; }
        public float NormalizedPosition { get; }
        public Vector2Int CellOffset { get; }
        public PortSide PortSide { get; }
        
        public PortIdentity(int index, float normalizedPosition, Vector2Int cellOffset, PortSide portSide)
        {
            Index = index;
            NormalizedPosition = normalizedPosition;
            CellOffset = cellOffset;
            PortSide = portSide;
        }
    }
}