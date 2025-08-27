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
        
        public float PreviewDamage(float damage)
        {
            float before = _health.Value;
            float after = _health.Clamp(before - damage);
            
            return before - after; 
        }

        public void ApplyDamage(float damage)
        {
            _health.Value -= damage;
        }
    }
}