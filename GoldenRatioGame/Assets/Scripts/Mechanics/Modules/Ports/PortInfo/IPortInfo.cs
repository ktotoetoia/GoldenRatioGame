using UnityEngine;

namespace IM.Modules
{
    public interface IPortInfo
    {
        Vector3 Position { get; }
        float EulerZRotation { get; }
    }
}