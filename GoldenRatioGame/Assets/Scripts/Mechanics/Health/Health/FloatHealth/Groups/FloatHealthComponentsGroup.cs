using System;
using System.Collections.Generic;
using IM.Values;

namespace IM.Health
{
    public class FloatHealthComponentsGroup : IFloatHealthComponentsGroup
    {
        private readonly List<IFloatHealth> _healthComponents = new();

        public IReadOnlyList<IFloatHealth> Components => _healthComponents;
        public ICappedValueReadOnly<float> Health => GetCurrentHealth();
        
        public void AddHealth(IFloatHealth healthComponent)
        {
            if (_healthComponents.Contains(healthComponent))
                throw new Exception("Health component already exists");
            
            _healthComponents.Add(healthComponent);
        }

        public void RemoveHealth(IFloatHealth healthComponent)
        {
            _healthComponents.Remove(healthComponent);
        }

        private ICappedValueReadOnly<float> GetCurrentHealth()
        {
            float totalMax = 0f;
            float totalCurrent = 0f;

            foreach (IFloatHealth component in _healthComponents)
            {
                totalMax += component.Health.MaxValue;
                float currentAboveMin = component.Health.Value - component.Health.MinValue;
                if (currentAboveMin > 0)
                    totalCurrent += currentAboveMin;
            }

            return new CappedValueReadOnly<float>(totalCurrent, 0, totalMax);
        }
        
        public HealthChangeResult TakeDamage(float damage) => ProcessHealthChange(damage,(x,y) => x.TakeDamage(y));
        public HealthChangeResult PreviewDamage(float damage) => ProcessHealthChange(damage,(x,y) => x.PreviewDamage(y));
        public HealthChangeResult RestoreHealth(float healing) =>ProcessHealthChange(healing,(x,y) => x.RestoreHealth(y));
        public HealthChangeResult PreviewHealing(float healing) => ProcessHealthChange(healing,(x,y) => x.PreviewHealing(y));
        
        private HealthChangeResult ProcessHealthChange(float value, Func<IFloatHealth, float,HealthChangeResult> apply)
        {
            HealthChangeResult result = new HealthChangeResult(value, value, 0);

            foreach (var healthComponent in _healthComponents)
            {
                if (result.Overflow < 0)
                    break;

                HealthChangeResult r = apply(healthComponent, result.Overflow);

                result = new HealthChangeResult(
                    result.PreMitigation,
                    result.PostMitigationUnclamped - r.Mitigated,
                    result.Applied + r.Applied);
            }

            return result;
        }
    }
}