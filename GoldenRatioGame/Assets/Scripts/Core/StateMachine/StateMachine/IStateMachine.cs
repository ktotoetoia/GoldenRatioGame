namespace IM.StateMachines
{
    public interface IStateMachine : IUpdatable
    {

    }

    public interface IUpdatable : IUpdate, IFixedUpdate
    {

    }

    public interface IUpdate
    {
        void Update();
    }

    public interface IFixedUpdate
    {
        void FixedUpdate();
    }
}