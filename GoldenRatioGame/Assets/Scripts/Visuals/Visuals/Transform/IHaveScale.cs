using UnityEngine;

namespace IM.Visuals
{
    public interface IHaveScale
    {
        Vector3 LossyScale { get; }
        Vector3 LocalScale { get; }
    }
}