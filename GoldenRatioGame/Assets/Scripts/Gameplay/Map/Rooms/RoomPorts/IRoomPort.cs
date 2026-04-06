using UnityEngine;

namespace IM.Map
{
    public interface IRoomPort
    {
        Vector3 EnterPosition { get; }
        Vector3 DeploymentPosition { get; }
        IRoom Origin { get; }
        IRoomPort Connection { get; }
        bool IsConnected { get; }
        bool IsOpen { get; set; }
    }
}