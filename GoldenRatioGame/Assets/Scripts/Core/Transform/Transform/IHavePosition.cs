using UnityEngine;

namespace IM.Transforms
{
    public interface IHavePosition
    {
        Vector3 Position { get; }
        Vector3 LocalPosition { get; }
    }
}