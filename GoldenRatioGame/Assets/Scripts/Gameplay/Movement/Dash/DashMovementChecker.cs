using UnityEngine;

namespace IM.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class DashMovementChecker : MonoBehaviour
    {
        [SerializeField] private float _movementThreshold = 0.1f;
        private Rigidbody2D _rigidbody;
        private IDash _dash;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _dash = GetComponent<IDash>();
        }

        private void FixedUpdate()
        {
            if (_dash.IsDashing) return;

            Vector3 currentVelocity = _rigidbody.linearVelocity;

            if (!(currentVelocity.sqrMagnitude > _movementThreshold * _movementThreshold)) return;
            
            Vector3 moveDirection = currentVelocity.normalized;

            _dash.Trigger(moveDirection);
        }
    }
}