using UnityEngine;

namespace IM.Map
{
    public interface IRoomPort
    {
        Vector3 EnterPosition { get; }
        Vector3 DeploymentPosition { get; }
        IGameObjectRoom Origin { get; }
        IRoomPort Connection { get; }
        bool IsConnected { get; }
        bool IsOpen { get; set; }
    }
}