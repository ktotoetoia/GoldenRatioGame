using UnityEngine;

namespace IM.Map
{
    public interface IRoomPort
    {
        Vector3 EnterPosition { get; }
        Vector3 DeploymentPosition { get; }
        IGameObjectRoom Origin { get; }
        IRoomPort Connection { get; }
        float NormalizedPosition { get; }
        PortSide PortSide { get; }
        bool IsConnected { get; }
        bool IsOpen { get; set; }
        Vector2Int CellOffset { get; }
        void SetDestination(IRoomPort destination);
        void Initialize(IGameObjectRoom origin);
    }
}