using System.Collections.Generic;
using System.Linq;

namespace IM.StateMachines
{
    public class State : IState
    {
        private readonly List<ITransition> _transitions = new List<ITransition>();

        public virtual void FixedUpdate()
        {

        }

        public virtual void OnEnter()
        {

        }

        public virtual void OnExit()
        {

        }

        public virtual void Update()
        {

        }

        public void AddTransition(ITransition transition)
        {
            _transitions.Add(transition);
        }

        public IEnumerable<ITransition> GetAvailableTransitions()
        {
            return _transitions.Where(x => x.CanTransition());
        }
    }
}