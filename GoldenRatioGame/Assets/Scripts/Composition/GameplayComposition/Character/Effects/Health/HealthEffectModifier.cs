using IM.Values;

namespace IM.Effects
{
    public class HealthEffectModifier :  IHealthEffectModifier
    {
        public ICappedValue<float> Health { get; }
        
        public HealthEffectModifier(ICappedValue<float> health)
        {
            Health = health;
        }
    }
}