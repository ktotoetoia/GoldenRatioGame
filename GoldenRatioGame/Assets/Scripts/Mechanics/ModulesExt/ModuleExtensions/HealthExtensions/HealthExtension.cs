using IM.Values;

namespace IM.Modules
{
    public class HealthExtension : IHealthExtension
    {
        public ICappedValue<float> Health { get; }
        
        public HealthExtension(int minHealth, int maxHealth,int currentHealth) :this(new CappedValue<float>(minHealth, maxHealth, currentHealth))
        {
            
        }
        
        public HealthExtension(ICappedValue<float> health)
        {
            Health = health;
        }
    }
}