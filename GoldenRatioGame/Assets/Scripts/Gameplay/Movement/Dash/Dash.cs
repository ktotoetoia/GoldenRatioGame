using UnityEngine;

namespace IM.Movement
{
    public class Dash : MonoBehaviour, IDash
    {
        [SerializeField] private AnimationCurve _dashCurve;
        [SerializeField] private float _dashDurationInSec = 0.2f;
        [SerializeField] private float _dashSpeed = 15f;
        private IVelocityModifier _velocityModifier;
        private bool _processDash;
        private float _dashTimer;
        private Vector3 _dashDirection;

        public bool IsDashing => _processDash;

        private void Awake()
        {
            _velocityModifier = GetComponent<IVelocityModifier>();
        }

        private void FixedUpdate()
        {
            if (!_processDash) return;

            _dashTimer += Time.fixedDeltaTime;
        
            float normalizedTime = Mathf.Clamp01(_dashTimer / _dashDurationInSec);
            float currentSpeed = _dashCurve.Evaluate(normalizedTime) * _dashSpeed;

            Vector3 dashVelocity = _dashDirection * currentSpeed;
            VelocityInfo velocityInfo = new VelocityInfo(VelocityAction.Add, dashVelocity);
        
            _velocityModifier.ChangeVelocity(velocityInfo);

            if (_dashTimer >= _dashDurationInSec)
            {
                ForceStop();
            }
        }

        public void Trigger(Vector3 direction)
        {
            if (_processDash) return;

            if (direction == Vector3.zero) return; 

            _dashDirection = direction.normalized;
            _dashTimer = 0f;
            _processDash = true;
            FixedUpdate();
        }

        public void ForceStop()
        {
            _processDash = false;
            _dashTimer = 0f;
        }

        private void OnDisable()
        {
            ForceStop();
        }
    }
}