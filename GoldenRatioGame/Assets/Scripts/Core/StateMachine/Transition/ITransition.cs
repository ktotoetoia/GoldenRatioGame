namespace IM.StateMachines
{
    public interface ITransition
    {
        IState From { get; }
        IState To { get; }

        bool CanTransition();
    }
}