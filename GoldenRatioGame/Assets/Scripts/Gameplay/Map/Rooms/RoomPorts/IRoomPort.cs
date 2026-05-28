using UnityEngine;

namespace IM.Map
{
    public interface IRoomPort
    {
        IPortIdentity PortIdentity { get; }
        Vector3 EnterPosition { get; }
        Vector3 DeploymentPosition { get; }
        IGameObjectRoom Origin { get; }
        IRoomPort Destination { get; }
        bool IsConnected { get; }
        bool IsOpen { get; set; }
        void SetDestination(IRoomPort destination);
        void Initialize(IGameObjectRoom origin,IPortIdentity portIdentity);
    }
}