using UnityEngine;

namespace IM.Transforms
{
    public class SinOffsetAnimator : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private bool _isAnimating = false;
        [Header("Motion")]
        [SerializeField] private float _period = 2f;

        [SerializeField] private float _range = 1f;

        [Header("Rotation")]
        [SerializeField] private float _rotationAngle = 15f;

        [SerializeField] private Vector3 _rotationAxis = Vector3.forward;

        [Header("Options")]
        [SerializeField] private bool _useUnscaledTime = false;
        [SerializeField] private float _phaseOffset = 0f;

        private void Update()
        {
            if (!_isAnimating)
            {
                _target.localPosition = default;
                _target.localRotation = Quaternion.identity;
                
                return;
            }
            
            float t = (_useUnscaledTime ? Time.unscaledTime : Time.time);

            float phase = (t + _phaseOffset) * Mathf.PI * 2f / _period;
            float sin = Mathf.Sin(phase);

            Vector3 posOffset = default;
            posOffset.x += sin * _range;
            _target.localPosition = posOffset;

            float angle = sin * _rotationAngle;
            Quaternion rotOffset = Quaternion.AngleAxis(angle, _rotationAxis.normalized);
            
            _target.localRotation = rotOffset;
        }
    }
}