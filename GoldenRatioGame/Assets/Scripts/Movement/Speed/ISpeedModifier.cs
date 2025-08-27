namespace IM.Movement
{
    public interface ISpeedModifier
    {
        public float GetModifiedValue(float value);
    }
}