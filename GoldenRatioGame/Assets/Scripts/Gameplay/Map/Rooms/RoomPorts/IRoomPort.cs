using UnityEngine;

namespace IM.Map
{
    public interface IRoomPort
    {
        Vector3 Position { get; }
        IRoom Origin { get; }
        IRoomPort Connection { get; }
        bool IsConnected { get; }
        bool IsOpen { get; }
    }
}