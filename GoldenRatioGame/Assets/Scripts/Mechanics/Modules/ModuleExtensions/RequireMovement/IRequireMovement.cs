using UnityEngine;

namespace IM.Modules
{
    public interface IRequireMovement : IModuleExtension
    {
        void UpdateCurrentVelocity(Vector2 velocity);
    }
}