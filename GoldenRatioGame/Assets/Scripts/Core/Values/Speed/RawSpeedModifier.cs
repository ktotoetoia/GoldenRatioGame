namespace IM.Values
{
    public class RawSpeedModifier : ISpeedModifier
    {
        public float Add { get; }

        public RawSpeedModifier(float add)
        {
            Add = add;
        }

        public float GetModifiedValue(float value)
        {
            return Add;
        }
    }
}