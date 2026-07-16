using System;
using System.Collections.Generic;
using IM.Values;

namespace IM.Health
{
    public class DividedFloatHealthValuesGroup : IFloatHealthValuesGroup, IFloatHealthEvents
    {
        private readonly List<ICappedValue<float>> _healthValues = new();
    
        public IReadOnlyList<ICappedValueReadOnly<float>> Values => _healthValues;
        public event Action<float> OnHealthChanged;
    
        public ICappedValueReadOnly<float> Health => GetCurrentHealth();
        public HealthChangeResult TakeDamage(float damage) => 
            ProcessHealthChange(damage, ApplyDamage);
        public HealthChangeResult PreviewDamage(float damage) => 
            ProcessHealthChange(damage, PreviewDamageInternal);
        public HealthChangeResult RestoreHealth(float healing) => 
            ProcessHealthChange(healing, ApplyHealing);
        public HealthChangeResult PreviewHealing(float healing) => 
            ProcessHealthChange(healing, PreviewHealingInternal);

        public void AddHealth(ICappedValue<float> healthBar)
        {
            if (_healthValues.Contains(healthBar)) throw new Exception("Health bar already exists");

            _healthValues.Add(healthBar);
        }

        public void RemoveHealth(ICappedValue<float> healthBar)
        {
            _healthValues.Remove(healthBar);
        }

        public bool Contains(ICappedValueReadOnly<float> healthBar)
        {
            return healthBar is ICappedValue<float> health && _healthValues.Contains(health);
        }

        private HealthChangeResult ProcessHealthChange(float totalValue, Func<ICappedValue<float>, float, float> apply)
        {
            if (totalValue < 0) throw new ArgumentException("Value cannot be negative.");

            if (_healthValues.Count == 0) return new HealthChangeResult(totalValue, totalValue, 0f);

            float dividedValue = totalValue / _healthValues.Count;
            float totalApplied = 0f;

            foreach (ICappedValue<float> health in _healthValues)
            {
                float used = apply(health, dividedValue);
                totalApplied += used;
            }
        
            OnHealthChanged?.Invoke(Health.Value);

            return new HealthChangeResult(totalValue, totalValue, totalApplied);
        }
    
        private float PreviewDamageInternal(ICappedValue<float> health, float amount)
        {
            float available = health.Value - health.MinValue;
            return Math.Min(available, amount);
        }

        private float ApplyDamage(ICappedValue<float> health, float amount)
        {
            float used = PreviewDamageInternal(health, amount);
            health.Value -= used;
            return used;
        }

        private float PreviewHealingInternal(ICappedValue<float> health, float amount)
        {
            float available = health.MaxValue - health.Value;
            return Math.Min(available, amount);
        }

        private float ApplyHealing(ICappedValue<float> health, float amount)
        {
            float used = PreviewHealingInternal(health, amount);
            health.Value += used;
        
            return used;
        }

        private ICappedValueReadOnly<float> GetCurrentHealth()
        {
            float totalMax = 0f;
            float totalCurrent = 0f;

            foreach (ICappedValue<float> health in _healthValues)
            {
                totalMax += health.MaxValue;
                float currentAboveMin = health.Value - health.MinValue;
                if (currentAboveMin > 0)
                    totalCurrent += currentAboveMin;
            }

            return new CappedValueReadOnly<float>(totalCurrent, 0, totalMax);
        }
    }
}