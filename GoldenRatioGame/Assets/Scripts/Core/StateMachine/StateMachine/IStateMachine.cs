namespace IM.StateMachines
{
    public interface IStateMachine : IUpdatable
    {
        void UpdateTransition();
    }
}