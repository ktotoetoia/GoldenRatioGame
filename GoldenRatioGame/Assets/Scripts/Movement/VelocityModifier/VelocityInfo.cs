using UnityEngine;

namespace TD.Movement
{
    public struct VelocityInfo
    {
        public Vector2 Velocity { get; set; }
        public VelocityAction Action { get; set; }

        public VelocityInfo(VelocityAction action, Vector2 velocity)
        {
            Action = action;
            Velocity = velocity;
        }
    }
}