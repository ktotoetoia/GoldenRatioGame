using UnityEngine;

namespace IM.Effects
{
    public class VelocityEffectModifier : IVelocityEffectModifier
    {
        private readonly Vector2 _direction;
        private readonly AnimationCurve _curve;
        private readonly float _startTime;
        private readonly float _magnitude;
        private readonly float _duration;
        
        public Vector2 Velocity
        { 
            get 
            {
                float elapsedTime = Time.time - _startTime;
                float curveMultiplier = _curve?.Evaluate(elapsedTime/ _duration) ?? 1f;
                
                return _direction * (_magnitude * curveMultiplier);
            }
        }
        
        public VelocityEffectModifier(Vector2 direction)
            : this(direction.normalized, AnimationCurve.Linear(0f, 1f, 1f, 0f), Time.time, direction.magnitude,0.5f)
        {
            
        }
        
        public VelocityEffectModifier(Vector2 direction, AnimationCurve curve, float startTime, float magnitude,float duration)
        {
            _direction = direction;
            _curve = curve;
            _startTime = startTime;
            _magnitude = magnitude;
            _duration = duration;
        }
    }
}