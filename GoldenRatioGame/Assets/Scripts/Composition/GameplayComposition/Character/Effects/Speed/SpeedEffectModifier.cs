using IM.Values;

namespace IM.Modules
{
    public class SpeedEffectModifier : ISpeedEffectModifier
    {
        public ISpeedModifier SpeedModifier { get; }
        
        public SpeedEffectModifier(ISpeedModifier speedModifier)
        {
            SpeedModifier = speedModifier;
        }
    }
}