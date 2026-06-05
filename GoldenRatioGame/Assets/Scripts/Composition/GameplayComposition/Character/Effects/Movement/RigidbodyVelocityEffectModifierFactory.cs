using System;
using UnityEngine;

namespace IM.Effects
{
    [Serializable]
    public class RigidbodyVelocityEffectModifierFactory : IEffectModifierFactory
    {
        [SerializeField] private float _magnitude = 3;
        [SerializeField] private float _duration;
        [SerializeField] private AnimationCurve _curve;
        [SerializeField] private float _degreesOffset;
        
        public IEffectModifier Create(IEffectContext context)
        {
            Rigidbody2D rb = context.Instigator.gameObject.GetComponent<Rigidbody2D>();
            Vector2 baseDirection = rb.linearVelocity.normalized;
            Quaternion rotation = Quaternion.AngleAxis(_degreesOffset, Vector3.forward);
            Vector2 modifiedDirection = rotation * baseDirection;
            
            return new VelocityEffectModifier(modifiedDirection, _curve,Time.time, _magnitude, _duration);
        }
    }
}