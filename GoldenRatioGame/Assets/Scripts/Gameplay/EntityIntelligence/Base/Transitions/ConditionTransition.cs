using System.Collections.Generic;
using System.Linq;
using IM.StateMachines;

namespace IM.EntityIntelligence
{
    public class ConditionTransition : ITransition
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

        public bool CanTransition() => _conditions.All(x => x.Check());
        public void BeforeTransition() { }
    }
}