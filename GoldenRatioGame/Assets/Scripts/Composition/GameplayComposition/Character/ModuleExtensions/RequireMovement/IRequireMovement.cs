using UnityEngine;

namespace IM.Modules
{
    public interface IRequireMovement : IExtension
    {
        void UpdateCurrentVelocity(Vector2 velocity);
    }
}