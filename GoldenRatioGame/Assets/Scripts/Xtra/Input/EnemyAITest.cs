using IM.Values;
using UnityEngine;

namespace IM.Inputs
{
    public class EnemyAITest : MonoBehaviour
    {
        [SerializeField] private CappedValue<float> _time = new(0.5f,2);
        [SerializeField] private float _timeBetweenMoving = 2f;
        private PlayerStateMachine _playerStateMachine;
        private bool _isMoving;
        private float _finishTime;
        private Vector3 _movingDirection;
        private System.Random _random;
        
        private void Awake()
        {
            _random = new System.Random();
            _playerStateMachine = GetComponent<PlayerStateMachine>();
            _playerStateMachine.ProvideMovementDirection = ProvideMovementDirection;
        }

        private Vector2 ProvideMovementDirection()
        {
            if (!_isMoving)
            {
                if (_finishTime + _timeBetweenMoving < Time.time)
                {
                    _isMoving = true;
                    _finishTime = (float)(Time.time + _random.NextDouble() + _time.MinValue % _time.MaxValue);
                    _movingDirection = GetBiasedDirection();
                }
                
                return Vector2.zero;
            }

            if (_finishTime < Time.time)
            {
                _isMoving = false;
            }
            
            Debug.DrawLine(transform.position, transform.position + _movingDirection, Color.red);
            return _movingDirection;
        }
        private Vector3 GetBiasedDirection()
        {
            Vector3 toZero = (-transform.position).normalized;

            Vector3 random = new Vector3(
                (float)(_random.NextDouble() * 2.0 - 1.0),
                (float)(_random.NextDouble() * 2.0 - 1.0),
                0f
            ).normalized;

            float distance = transform.position.magnitude;

            float weight = Mathf.Clamp01(distance / 10f);

            return Vector3.Lerp(random, toZero, weight);
        }
    }
}