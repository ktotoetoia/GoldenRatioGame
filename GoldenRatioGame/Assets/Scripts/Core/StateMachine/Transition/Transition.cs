using System;

namespace IM.StateMachines
{
    public class Transition : ITransition
    {
        private Func<bool> _condition;

        public IState From { get; private set; }
        public IState To { get; private set; }

        public Transition(IState from, IState to, Func<bool> condition)
        {
            From = from;
            To = to;

            _condition = condition;
        }

        public bool CanTransition()
        {
            return _condition();
        }
    }
}