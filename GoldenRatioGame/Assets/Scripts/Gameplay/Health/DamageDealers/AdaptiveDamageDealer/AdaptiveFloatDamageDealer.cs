using UnityEngine;

namespace IM.Health
{
    public class AdaptiveFloatDamageDealer : IFloatDamageDealer
    {
        private readonly float _minDamage;
        private readonly float _maxDamage;
        private readonly float _avgDamage;

        private float _damageDebt = 0f; 

        private const float CorrectionFactor = 0.35f; 

        public AdaptiveFloatDamageDealer(float minDamage, float maxDamage, float avgDamage)
        {
            _minDamage = minDamage;
            _maxDamage = maxDamage;
            _avgDamage = avgDamage;
        }

        public float Damage
        {
            get => _avgDamage;
            set {}
        }

        public HealthChangeResult PreviewDamage(IDamageable target)
        {
            float expectedNextHit = Mathf.Clamp(_avgDamage - (_damageDebt * CorrectionFactor), _minDamage, _maxDamage);
            return target.PreviewDamage(expectedNextHit);
        }

        public HealthChangeResult DealDamage(IDamageable target)
        {
            float finalDamage = RollAdaptiveDamage(); 
            return target.TakeDamage(finalDamage);
        }

        private float RollAdaptiveDamage()
        {
            float currentTarget = _avgDamage - (_damageDebt * CorrectionFactor);

            currentTarget = Mathf.Clamp(currentTarget, _minDamage, _maxDamage);

            float roll1 = Random.Range(_minDamage, currentTarget);
            float roll2 = Random.Range(currentTarget, _maxDamage);
            float rolledDamage = (roll1 + roll2) / 2f; 
            
            rolledDamage = Mathf.Clamp(rolledDamage, _minDamage, _maxDamage);
            
            float deviation = rolledDamage - _avgDamage;
            _damageDebt += deviation;

            return rolledDamage;
        }
    }
}