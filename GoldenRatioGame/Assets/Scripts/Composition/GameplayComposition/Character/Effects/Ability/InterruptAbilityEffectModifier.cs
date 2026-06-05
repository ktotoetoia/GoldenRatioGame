namespace IM.Effects
{
    public class InterruptAbilityEffectModifier : IEffectModifier
    {
        public bool Interrupts { get; set; }
        
        public InterruptAbilityEffectModifier(bool interrupts)
        {
            Interrupts = interrupts;
        }
    }
}