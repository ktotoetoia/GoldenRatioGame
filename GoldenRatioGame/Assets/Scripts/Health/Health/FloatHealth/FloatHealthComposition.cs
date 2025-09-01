using System;
using System.Collections.Generic;
using IM.Economy;
using UnityEngine;

namespace IM.Health
{
    public class FloatHealthComposition : IFloatHealthComposition
    {
        private List<IFloatHealth> _healthComponents = new();

        public IReadOnlyList<IFloatHealth> HealthComponents => _healthComponents;

        public ICappedValueReadOnly<float> Health => GetCurrentHealth();

        public DamageResult ApplyDamage(float damage) => ProcessDamage(damage, apply: true);
        public DamageResult PreviewDamage(float damage) => ProcessDamage(damage, apply: false);
        public HealingResult ApplyHealing(float healing) => ProcessHealing(healing, apply: true);
        public HealingResult PreviewHealing(float healing) => ProcessHealing(healing, apply: false);

        private ICappedValueReadOnly<float> GetCurrentHealth()
        {
            float totalMax = 0f;
            float totalCurrent = 0f;

            foreach (var component in _healthComponents)
            {
                totalMax += component.Health.MaxValue;
                float currentAboveMin = component.Health.Value - component.Health.MinValue;
                if (currentAboveMin > 0)
                    totalCurrent += currentAboveMin;
            }

            return new CappedValueReadOnly<float>(totalCurrent, 0, totalMax);
        }

        private DamageResult ProcessDamage(float damage, bool apply)
        {
            if (damage < 0) throw new ArgumentException("Damage cannot be negative. Use healing instead.");

            float remaining = damage;
            float totalApplied = 0f;
            float before = GetCurrentHealth().Value;

            for (int i = _healthComponents.Count - 1; i >= 0 && remaining > 0; i--)
            {
                var comp = _healthComponents[i];
                float available = comp.Health.Value - comp.Health.MinValue;
                if (available <= 0) continue;

                float applyAmount = Mathf.Min(remaining, available);
                if (apply) comp.ApplyDamage(applyAmount);

                totalApplied += applyAmount;
                remaining -= applyAmount;
            }

            float after = apply ? GetCurrentHealth().Value : before - totalApplied;
            
            return new DamageResult(before, after, damage, damage - totalApplied);
        }

        private HealingResult ProcessHealing(float healing, bool apply)
        {
            if (healing < 0) throw new ArgumentException("Healing cannot be negative. Use damage instead.");

            float remaining = healing;
            float totalApplied = 0f;
            float before = GetCurrentHealth().Value;

            for (int i = 0; i < _healthComponents.Count && remaining > 0; i++)
            {
                var comp = _healthComponents[i];
                float available = comp.Health.MaxValue - comp.Health.Value;
                if (available <= 0) continue;

                float applyAmount = Mathf.Min(remaining, available);
                if (apply) comp.ApplyHealing(applyAmount);

                totalApplied += applyAmount;
                remaining -= applyAmount;
            }

            float after = apply ? GetCurrentHealth().Value : before + totalApplied;
            
            return new HealingResult(before, after, healing, healing - totalApplied);
        }
    }
}
