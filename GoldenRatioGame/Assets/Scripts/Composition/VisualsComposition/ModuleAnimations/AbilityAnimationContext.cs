using UnityEngine;

namespace IM.Visuals
{
    public class AbilityAnimationContext : IAnimationContext
    {
        public float FocusTime { get;  }
        public Vector2 Direction { get;  }
        
        public AbilityAnimationContext(Vector2 direction, float focusTime)
        {
            Direction = direction;
            FocusTime = focusTime;
        }
    }
}