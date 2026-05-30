namespace IM.StateMachines
{
    public interface ITransitionEvents
    {
        void OnFromStateStarted();
        void OnFromStateExited();
    }
}