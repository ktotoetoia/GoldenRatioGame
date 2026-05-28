using UnityEngine;

namespace IM.Movement
{
    public struct VelocityInfo
    {
        public Vector2 Velocity { get; set; }
        public VelocityAction Action { get; set; }
        
        public VelocityInfo(Vector2 velocity) : this(VelocityAction.Add,velocity)
        {
            
        }
        
        public VelocityInfo(VelocityAction action, Vector2 velocity)
        {
            Action = action;
            Velocity = velocity;
        }
    }
}