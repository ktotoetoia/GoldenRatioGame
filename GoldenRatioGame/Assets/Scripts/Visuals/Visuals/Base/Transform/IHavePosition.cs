using UnityEngine;

namespace IM.Visuals
{
    public interface IHavePosition
    {
        Vector3 Position { get; }
        Vector3 LocalPosition { get; }
    }
}