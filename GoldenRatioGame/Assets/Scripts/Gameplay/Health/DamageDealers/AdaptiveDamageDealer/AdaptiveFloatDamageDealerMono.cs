using IM.Values;
using UnityEngine;

namespace IM.Health
{
    public class AdaptiveFloatDamageDealerMono : MonoBehaviour, IFloatDamageDealer
    {
        [SerializeField] private CappedValue<float> _damage;
        
        private IFloatDamageDealer _floatDamageDealer;

        private void Awake()
        {
            _floatDamageDealer = new AdaptiveFloatDamageDealer(_damage.MinValue,_damage.MaxValue, _damage.Value);
        }

        public HealthChangeResult PreviewDamage(IDamageable target) => _floatDamageDealer.PreviewDamage(target);
        public HealthChangeResult DealDamage(IDamageable target) => _floatDamageDealer.DealDamage(target);

        public float Damage
        {
            get => _floatDamageDealer.Damage;
            set => _floatDamageDealer.Damage = value;
        }
    }
}