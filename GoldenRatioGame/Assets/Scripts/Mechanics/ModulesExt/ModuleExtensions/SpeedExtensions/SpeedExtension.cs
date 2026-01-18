using IM.Values;

namespace IM.Modules
{
    public class SpeedExtension : ISpeedExtension
    {
        public ISpeedModifier SpeedModifier { get; set; }

        public SpeedExtension(ISpeedModifier speedModifier)
        {
            SpeedModifier = speedModifier;
        }
    }
}