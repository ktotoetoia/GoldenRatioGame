namespace IM.Abilities
{
    public interface IInterruptable
    {
        bool CanInterrupt { get; }
        bool TryInterrupt();
        void Interrupt();
    }
}