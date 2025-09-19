using System.Collections.Generic;

namespace IM.StateMachines
{
    public interface IState : IUpdatable
    {
        void OnEnter();
        void OnExit();
        void AddTransition(ITransition transition);
        IEnumerable<ITransition> GetAvailableTransitions();
    }
}