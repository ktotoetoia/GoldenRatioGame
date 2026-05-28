using UnityEngine;

namespace IM.Effects
{
    public interface IVelocityEffectModifier : IEffectModifier
    {
        Vector2 Velocity { get;  }
    }
}