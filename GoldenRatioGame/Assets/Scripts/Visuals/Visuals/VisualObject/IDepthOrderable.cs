using UnityEngine;

namespace IM.Visuals
{
    public interface IDepthOrderable : IOrderable 
    {
        Vector3 ReferencePoint { get; }
    }
}