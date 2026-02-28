namespace IM.Abilities
{
    public class AbilityFocusTimeDescriptor : IAbilityFocusTimeDescriptor
    {
        public float FocusTime { get; set; }
        
        public AbilityFocusTimeDescriptor(float focusTime = 0.5f)
        {
            FocusTime = focusTime;
        }
    }
}