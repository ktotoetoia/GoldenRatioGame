using System;
using System.Collections.Generic;
using IM.Economy;

namespace IM.Health
{
    public class FloatHealthValuesGroup : IFloatHealthValuesGroup
    {
        private readonly List<ICappedValue<float>> _healthValues = new();

        public IReadOnlyList<ICappedValueReadOnly<float>> Values => _healthValues;
        public ICappedValueReadOnly<float> Health => GetCurrentHealth();

        public void AddHealth(ICappedValue<float> healthBar)
        {
            if (_healthValues.Contains(healthBar))
                throw new Exception("Health bar already exists");

            _healthValues.Add(healthBar);
        }

        public void RemoveHealth(ICappedValue<float> healthBar)
        {
            _healthValues.Remove(healthBar);
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

        public HealthChangeResult TakeDamage(float damage) => 
            ProcessHealthChange(damage, ApplyDamage);

        public HealthChangeResult PreviewDamage(float damage) => 
            ProcessHealthChange(damage, PreviewDamageInternal);

        public HealthChangeResult RestoreHealth(float healing) => 
            ProcessHealthChange(healing, ApplyHealing);

        public HealthChangeResult PreviewHealing(float healing) => 
            ProcessHealthChange(healing, PreviewHealingInternal);

        private HealthChangeResult ProcessHealthChange(
            float value,
            Func<ICappedValue<float>, float, float> apply)
        {
            if (value < 0)
                throw new ArgumentException("Value cannot be negative.");

            float remaining = value;
            float applied = 0f;

            foreach (var health in _healthValues)
            {
                if (remaining <= 0) break;

                float used = apply(health, remaining);
                applied += used;
                remaining -= used;
            }

            return new HealthChangeResult(value, value, applied);
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
    }
}