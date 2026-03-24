using UnityEngine;

namespace IM.Health
{
    public class RawFloatDamageDealerMono : MonoBehaviour, IFloatDamageDealer
    {
        [SerializeField] private float _damage;
        private IFloatDamageDealer _floatDamageDealer;

        private void Awake()
        {
            _floatDamageDealer = new RawFloatDamageDealer(_damage);
        }

        public HealthChangeResult PreviewDamage(IDamageable target)
        {
            return _floatDamageDealer.PreviewDamage(target);
        }

        public HealthChangeResult DealDamage(IDamageable target)
        {
            return _floatDamageDealer.DealDamage(target);
        }

        public float Damage
        {
            get => _floatDamageDealer.Damage;
            set => _floatDamageDealer.Damage = value;
        }
    }
}