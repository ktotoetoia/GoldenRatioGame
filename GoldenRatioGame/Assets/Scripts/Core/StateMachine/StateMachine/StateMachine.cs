using System.Linq;

namespace IM.StateMachines
{
    public class StateMachine : IStateMachine
    {
        private IState _currentState;

        public IState CurrentState
        {
            get => _currentState;
            
            private set
            {
                _currentState.OnExit();
                _currentState = value;
                value.OnEnter();
            }
        }

        public StateMachine(IState startState)
        {
            _currentState = startState;
        }

        public void Update()
        {
            if (CurrentState is IUpdate update)
            {
                update.Update();
            }
            
            TryTransition();
        }

        public void FixedUpdate()
        {
            if (CurrentState is IFixedUpdate fixedUpdate)
            {
                fixedUpdate.FixedUpdate();
            }
            
            TryTransition();
        }

        private void TryTransition()
        {
            ITransition transition = _currentState.GetAvailableTransitions().FirstOrDefault();

            if (transition == null) return;
            
            CurrentState = transition.To;
        }
    }
}