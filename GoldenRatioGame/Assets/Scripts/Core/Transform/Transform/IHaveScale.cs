using UnityEngine;

namespace IM.Transforms
{
    public interface IHaveScale
    {
        Vector3 LossyScale { get; }
        Vector3 LocalScale { get; }
    }
}