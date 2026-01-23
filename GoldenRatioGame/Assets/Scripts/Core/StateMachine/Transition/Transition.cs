using System;

namespace IM.StateMachines
{
    public class Transition : ITransition
    {
        private readonly Func<bool> _condition;

        public IState From { get; }
        public IState To { get; }

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