using UnityEngine;

namespace IM.Visuals
{
    public class MoveAnimationContext : IAnimationContext
    {
        public Vector2 Velocity { get; }
        public float Offset { get; }
        
        public MoveAnimationContext(Vector2 velocity, float offset)
        {
            Velocity = velocity;
            Offset = offset;
        }
    }
}