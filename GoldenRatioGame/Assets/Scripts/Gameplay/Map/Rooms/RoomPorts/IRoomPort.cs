using UnityEngine;

namespace IM.Map
{
    public interface IRoomPort
    {
        IPortIdentity PortIdentity { get; }
        Vector3 EnterPosition { get; }
        Vector3 DeploymentPosition { get; }
        IGameObjectRoom Origin { get; }
        IRoomPort Connection { get; }
        bool IsConnected { get; }
        bool IsOpen { get; set; }
        void SetDestination(IRoomPort destination);
        void Initialize(IGameObjectRoom origin,IPortIdentity portIdentity);
    }

    public interface IPortIdentity
    {
        int Index { get; }
        float NormalizedPosition { get; }
        Vector2Int CellOffset { get; }
        PortSide PortSide { get; }
    }

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