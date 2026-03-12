namespace IM.Movement
{
    public interface IControllableMovement : IVectorMovement
    {
        bool Active { get; }

        void Stop();
        void Halt();
        void Activate();
    }
}