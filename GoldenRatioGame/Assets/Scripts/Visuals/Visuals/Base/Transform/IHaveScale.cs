using UnityEngine;

namespace IM.Visuals
{
    public interface IHaveScale
    {
        Vector3 Scale { get; }
        Vector3 LocalScale { get; }
    }
}