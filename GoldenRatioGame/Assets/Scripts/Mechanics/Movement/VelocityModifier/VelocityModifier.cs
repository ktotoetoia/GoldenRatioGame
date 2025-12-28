using UnityEngine;

namespace IM.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    [DefaultExecutionOrder(10)]
    public class VelocityModifier : MonoBehaviour, IVelocityModifier
    {
        private VelocityInfo _velocity;
        private Rigidbody2D _rigidbody;
        
        public Vector2 LastAppliedNonZeroVelocity { get; private set; }
        
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            _rigidbody.linearVelocity = _velocity.Velocity;

            if(_velocity.Velocity != Vector2.zero)
                LastAppliedNonZeroVelocity = _rigidbody.linearVelocity;
 
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
    }
}