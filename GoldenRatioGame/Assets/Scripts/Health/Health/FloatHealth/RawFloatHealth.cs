using System;
using IM.Economy;
using UnityEngine;

namespace IM.Health
{
    public class RawFloatHealth : IFloatHealth
    {
        private readonly CappedValue<float> _health;
        
        public ICappedValueReadOnly<float> Health => _health;
        
        public RawFloatHealth(float minHealth, float maxHealth, float currentHealth) 
            : this(new CappedValue<float>(minHealth, maxHealth, currentHealth))
        {
            
        }
        
        public RawFloatHealth(CappedValue<float> health)
        {
            _health = health;
        }
        
        public HealthChangeResult PreviewDamage(float damage)
        {
            if (damage < 0)
                throw new ArgumentException("Damage cannot be negative. use Preview Healing instead");

            float applied = _health.Value < damage ? _health.Value : damage;
            
            return new HealthChangeResult(damage, damage,applied);
        }

        public HealthChangeResult TakeDamage(float damage)
        {
            if (damage < 0)
                throw new ArgumentException("Damage cannot be negative. use Apply Healing instead");
            
            HealthChangeResult result = PreviewDamage(damage);
            
            _health.Value -= result.Applied;
            
            return result;
        }

        public HealthChangeResult PreviewHealing(float healing)
        {
            if (healing < 0)
                throw new ArgumentException("Healing cannot be negative. use PreviewDamage instead.");
            
            float applied = _health.MaxValue < _health.Value + healing ? _health.MaxValue - _health.Value: healing;
            
            return new HealthChangeResult(healing, healing, applied);
        }

        public HealthChangeResult RestoreHealth(float healing)
        {
            if (healing < 0)
                throw new ArgumentException("Healing cannot be negative. use ApplyDamage instead.");

            HealthChangeResult result = PreviewHealing(healing);

            _health.Value += result.Applied;

            return result;
        }
    }
}