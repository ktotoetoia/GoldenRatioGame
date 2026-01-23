using UnityEngine;

namespace IM.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    [DefaultExecutionOrder(VelocityModifierApplyVelocityOrder)]
    public class VelocityModifier : MonoBehaviour, IVelocityModifier
    {
        private VelocityInfo _velocity;
        private Rigidbody2D _rigidbody;

        public Vector2 VelocityToApply => _velocity.Velocity;
        
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            _rigidbody.linearVelocity = _velocity.Velocity;
            _velocity = default;
        }

        public void ChangeVelocity(VelocityInfo velocityInfo)
        {
            if (velocityInfo.Action == VelocityAction.Add && _velocity.Action == VelocityAction.Add)
            {
                _velocity.Velocity += velocityInfo.Velocity;

                return;
            }
            if (velocityInfo.Action == VelocityAction.Override)
            {
                _velocity = velocityInfo;
            }
        }

        private const int VelocityModifierApplyVelocityOrder = 10;
    }
}