namespace IM.StateMachines
{
    public interface IStateMachine : IUpdatable
    {
        IState CurrentState { get; }
        
        void UpdateTransition();
    }
}