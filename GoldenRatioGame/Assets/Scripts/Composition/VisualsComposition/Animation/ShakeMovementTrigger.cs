using UnityEngine;

namespace IM.Visuals
{
    public sealed class ShakeMovementTrigger : MonoBehaviour, IRequireModuleVisualObjectInitialization
    {
        [SerializeField] private Transform _target;
        [SerializeField] private bool _triggerShake;
        [SerializeField] private float _shakeDuration = 0.2f;
        [SerializeField] private float _frequency = 40f;
        [SerializeField] private Vector2 _magnitude = new Vector2(0.1f, 0.1f);

        private Vector3 _baseLocalPosition;
        private float _shakeElapsed;
        private bool _isInitialized;
        private bool _isShaking;

        public void OnModuleVisualObjectInitialized(IModuleVisualObject moduleVisualObject)
        {
            if (!_target)
                return;

            _baseLocalPosition = _target.localPosition;
            _isInitialized = true;
        }

        private void Update()
        {
            if (!_target || !_isInitialized)
                return;

            if (_triggerShake)
            {
                _triggerShake = false;
                StartShake();
            }

            if (!_isShaking)
                return;

            if (_shakeDuration <= 0f)
            {
                StopShake();
                return;
            }

            _shakeElapsed += Time.deltaTime;

            if (_shakeElapsed >= _shakeDuration)
            {
                StopShake();
                return;
            }

            float normalizedTime = _shakeElapsed / _shakeDuration;
            float envelope = 1f - normalizedTime;

            float phase = _shakeElapsed * _frequency * Mathf.PI * 2f;
            float x = Mathf.Sin(phase) * _magnitude.x * envelope;
            float y = Mathf.Cos(phase * 0.9f) * _magnitude.y * envelope;

            _target.localPosition = _baseLocalPosition + new Vector3(x, y, 0f);
        }

        private void StartShake()
        {
            _shakeElapsed = 0f;
            _isShaking = true;
            _target.localPosition = _baseLocalPosition;
        }

        private void StopShake()
        {
            _isShaking = false;
            _shakeElapsed = 0f;
            _target.localPosition = _baseLocalPosition;
        }

        private void OnDisable()
        {
            _triggerShake = false;
            _isShaking = false;
            _shakeElapsed = 0f;

            if (_target && _isInitialized)
                _target.localPosition = _baseLocalPosition;
        }
    }
}