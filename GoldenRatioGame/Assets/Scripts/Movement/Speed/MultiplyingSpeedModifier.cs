namespace IM.Movement
{
    public class MultiplyingSpeedModifier : ISpeedModifier
    {
        public float Multiplier { get; }

        public MultiplyingSpeedModifier(float multiplier)
        {
            Multiplier = multiplier;
        }

        public float GetModifiedValue(float value)
        {
            return value * Multiplier;
        }
    }
}