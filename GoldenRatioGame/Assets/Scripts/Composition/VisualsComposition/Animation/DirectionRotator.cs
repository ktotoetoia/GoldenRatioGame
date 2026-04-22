using IM.Abilities;
using UnityEngine;

namespace IM.Visuals
{
    public sealed class DirectionRotator : MonoBehaviour, IRequireModuleVisualObjectInitialization
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _offsetAngle;
        [SerializeField] private bool _animate;

        [SerializeField] private float _rotationSpeed = 720f;
        [SerializeField] private AnimationCurve _rotationSpeedCurveOnTarget = AnimationCurve.Linear(0f, 0.25f, 1f, 1f);
        [SerializeField] private AnimationCurve _rotationSpeedCurveOnDefault = AnimationCurve.Linear(0f, 0.25f, 1f, 1f);

        [SerializeField] private bool _shakeOnTarget;
        [SerializeField] private bool _shakeOnDefault;
        [SerializeField] private float _shakeFrequency = 20f;
        [SerializeField] private float _shakeMagnitude = 5f;

        private IAbilityContainer _abilityContainer;
        private Quaternion _internalRotation;
        private float _shakeTimer;

        public void OnModuleVisualObjectInitialized(IModuleVisualObject moduleVisualObject)
        {
            _abilityContainer = moduleVisualObject.Owner.Extensions.Get<IAbilityContainer>();

            if (_target)
                _internalRotation = _target.rotation;
        }

        private void Update()
        {
            if (!_target) return;

            bool isContextValid = _animate && _abilityContainer?.Ability is IFocusPointProvider;
            Quaternion goalRotation = CalculateGoalRotation(isContextValid);
            
            
            float angleToTarget = Quaternion.Angle(_internalRotation, goalRotation);
            if (angleToTarget > 0.001f)
            {
                float normalizedDistance = Mathf.Clamp01(angleToTarget / 180f);
                float speedMultiplier = EvaluateSpeedCurve(isContextValid, normalizedDistance);

                _internalRotation = Quaternion.RotateTowards(
                    _internalRotation,
                    goalRotation,
                    _rotationSpeed * speedMultiplier * Time.deltaTime
                );
            }
            else
            {
                _internalRotation = goalRotation;
            }

            _target.rotation = _internalRotation;

            bool isAtDestination = Quaternion.Angle(_internalRotation, goalRotation) < 0.01f;
            bool canShake = isContextValid ? _shakeOnTarget : _shakeOnDefault;

            if (isAtDestination && canShake)
            {
                _shakeTimer += Time.deltaTime;
                float shakeOffset = Mathf.Sin(_shakeTimer * _shakeFrequency) * _shakeMagnitude;
                _target.rotation *= Quaternion.Euler(0f, 0f, shakeOffset);
            }
            else
            {
                _shakeTimer = 0f;
            }
        }
        
        private float EvaluateSpeedCurve(bool isContextValid, float normalizedDistance)
        {
            AnimationCurve curve = isContextValid ? _rotationSpeedCurveOnTarget : _rotationSpeedCurveOnDefault;
            return curve != null ? Mathf.Max(0f, curve.Evaluate(normalizedDistance)) : 1f;
        }

        private Quaternion CalculateGoalRotation(bool isContextValid)
        {
            if (!isContextValid)
                return Quaternion.identity;

            IFocusPointProvider focusPointProvider = (IFocusPointProvider)_abilityContainer.Ability;
            Vector3 direction = focusPointProvider.GetFocusDirection().normalized;

            if (direction.sqrMagnitude < 0.0001f)
                return _internalRotation;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + _offsetAngle;
            return Quaternion.AngleAxis(angle, Vector3.forward);
        }

        private void OnDisable()
        {
            _animate = false;
            _shakeTimer = 0f;
        }
    }
}