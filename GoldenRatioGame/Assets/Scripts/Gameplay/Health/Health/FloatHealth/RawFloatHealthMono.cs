using System;
using IM.Values;
using UnityEngine;

namespace IM.Health
{
    public class RawFloatHealthMono : MonoBehaviour, IFloatHealth,IFloatHealthEvents
    {
        [SerializeField] private CappedValue<float> _health;
        
        private IFloatHealth _floatHealth;
        private IFloatHealth FloatHealth => _floatHealth ??= new RawFloatHealth(_health);
        public event Action<float> OnHealthChanged;

        public ICappedValueReadOnly<float> Health => FloatHealth.Health;
        
        public HealthChangeResult PreviewDamage(float incomingDamage)
        {
            return FloatHealth.PreviewDamage(incomingDamage);
        }

        public HealthChangeResult TakeDamage(float damage)
        {
            HealthChangeResult healthChangeResult = FloatHealth.TakeDamage(damage);
            
            OnHealthChanged?.Invoke(_floatHealth.Health.Value);
            
            return healthChangeResult;
        }

        public HealthChangeResult PreviewHealing(float healing)
        {
            return FloatHealth.PreviewHealing(healing);
        }

        public HealthChangeResult RestoreHealth(float healing)
        {
            HealthChangeResult healthChangeResult = FloatHealth.RestoreHealth(healing);

            OnHealthChanged?.Invoke(_floatHealth.Health.Value);
            
            return healthChangeResult;
        }
    }
}