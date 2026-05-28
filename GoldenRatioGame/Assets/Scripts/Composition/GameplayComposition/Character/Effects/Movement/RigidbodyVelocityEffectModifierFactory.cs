using UnityEngine;

namespace IM.Effects
{
    [CreateAssetMenu(menuName = "Effects/Modifiers/RigidBody Direction Velocity Effect Modifier")]
    public class RigidbodyVelocityEffectModifierFactory : EffectModifierFactory
    {
        [SerializeField] private float _magnitude = 3;
        [SerializeField] private float _duration;
        [SerializeField] private AnimationCurve _curve;
        [SerializeField] private float _degreesOffset;
        
        public override IEffectModifier Create(IEffectContext context)
        {
            Rigidbody2D rb = context.Instigator.gameObject.GetComponent<Rigidbody2D>();
            Vector2 baseDirection = rb.linearVelocity.normalized;
            Quaternion rotation = Quaternion.AngleAxis(_degreesOffset, Vector3.forward);
            Vector2 modifiedDirection = rotation * baseDirection;
            
            return new VelocityEffectModifier(modifiedDirection, _curve,Time.time, _magnitude, _duration);
        }
    }
}