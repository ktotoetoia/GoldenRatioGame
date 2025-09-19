using System.Linq;

namespace IM.StateMachines
{
    public class StateMachine : IStateMachine
    {
        private IState _currentState;

        public IState CurrentState
        {
            get
            {
                return _currentState;
            }

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
            _currentState.Update();
            TryTransition();
        }

        public void FixedUpdate()
        {
            _currentState.FixedUpdate();
            TryTransition();
        }

        private void TryTransition()
        {
            ITransition transition = _currentState.GetAvailableTransitions().FirstOrDefault();

            if (transition != null)
            {
                CurrentState = transition.To;
            }
        }
    }
}