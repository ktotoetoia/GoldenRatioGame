using System;
using IM.Economy;

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
        
        public DamageResult PreviewDamage(float damage)
        {
            if (damage < 0)
                throw new ArgumentException("Damage cannot be negative. use Preview Healing instead");
            
            float before = _health.Value;
            float unclampedAfter = before - damage;
            float after = _health.Clamp(unclampedAfter);

            return new DamageResult(before, after,damage,0);
        }

        public DamageResult ApplyDamage(float damage)
        {
            if (damage < 0)
                throw new ArgumentException("Damage cannot be negative. use Apply Healing instead");
            
            DamageResult result = PreviewDamage(damage);
            
            _health.Value -= damage;
            
            return result;
        }

        public HealingResult PreviewHealing(float healing)
        {
            if (healing < 0)
                throw new ArgumentException("Healing cannot be negative. use PreviewDamage instead.");

            float before = _health.Value;
            float unclampedAfter = before + healing;
            float after = _health.Clamp(unclampedAfter);

            float applied = after - before;
            float mitigated = healing - applied;

            return new HealingResult(before, after, healing, mitigated);
        }

        public HealingResult ApplyHealing(float healing)
        {
            if (healing < 0)
                throw new ArgumentException("Healing cannot be negative. use ApplyDamage instead.");

            HealingResult result = PreviewHealing(healing);

            _health.Value = _health.Clamp(_health.Value + healing);

            return result;
        }
    }
}