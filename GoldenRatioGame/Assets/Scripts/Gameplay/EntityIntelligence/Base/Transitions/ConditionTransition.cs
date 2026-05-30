using System.Collections.Generic;
using System.Linq;
using IM.StateMachines;
using UnityEngine;

namespace IM.EntityIntelligence
{
    public class ConditionTransition : ITransition, ITransitionEvents
    {
        private readonly List<ICondition> _conditions;

        public IState From { get; }
        public IState To { get; }
        
        public ConditionTransition(IState from, IState to, IEnumerable<ICondition> conditions)
        {
            From = from;
            To = to;
            _conditions = conditions.ToList();
        }

        public bool CanTransition()
        {
            return _conditions.All(x => x.Check());
        }

        public void OnBeforeUsedForTransition() { }
        
        public void OnFromStateStarted()
        {
            foreach(var condition in _conditions) condition.Start();
        }

        public void OnFromStateExited()
        {
            foreach(var condition in _conditions) condition.Finish();
        }
    }
}