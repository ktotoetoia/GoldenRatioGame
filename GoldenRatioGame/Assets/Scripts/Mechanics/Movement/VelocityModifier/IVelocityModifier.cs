using UnityEngine;

namespace IM.Movement
{
    public interface IVelocityModifier
    {
        public Vector2 VelocityToApply { get; }
        
        public void ChangeVelocity(VelocityInfo velocityInfo);
    }
}