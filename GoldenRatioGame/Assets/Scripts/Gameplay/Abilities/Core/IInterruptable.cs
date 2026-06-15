namespace IM.Abilities
{
    public interface IInterruptable
    {
        bool CanInterrupt(InterruptionCause cause);
        bool TryInterrupt(InterruptionCause cause);
        void Interrupt(InterruptionCause cause);
    }

    public enum InterruptionCause
    {
        None,
        UserCancelled,
        Forced,
    }
}