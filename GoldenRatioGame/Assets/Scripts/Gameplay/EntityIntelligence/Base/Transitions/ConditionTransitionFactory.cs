using System.Collections.Generic;
using System.Linq;
using IM.StateMachines;
using UnityEngine;

namespace IM.EntityIntelligence
{
    [CreateAssetMenu(menuName = "Entity Intelligence/Condition Transition Factory")]
    public class ConditionTransitionFactory : TransitionFactory
    {
        [SerializeReferenceDropdown] [SerializeReference] private List<IConditionFactory> _conditionFactories;
        
        public override ITransition Create(GameObject param1, IState from, IState to)
        {
            return new ConditionTransition(from,to,_conditionFactories.Select(x => x.Create(param1)));
        }
    }
}