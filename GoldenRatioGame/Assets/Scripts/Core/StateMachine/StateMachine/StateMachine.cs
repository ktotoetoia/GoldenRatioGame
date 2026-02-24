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
                if(_currentState == value) return;
                
                _currentState?.OnExit();
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
            if (CurrentState is IUpdate update) update.Update();
            
            TryTransition();
        }

        public void FixedUpdate()
        {
            if (CurrentState is IFixedUpdate fixedUpdate) fixedUpdate.FixedUpdate();

            TryTransition();
        }

        private bool TryTransition()
        {
            ITransition transition = _currentState.GetAvailableTransitions().FirstOrDefault();

            if (transition == null) return false;
            
            transition.BeforeTransition();
            CurrentState = transition.To;
            
            return true;
        }
    }
}