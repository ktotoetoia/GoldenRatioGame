using System.Collections.Generic;
using System.Linq;
using IM.StateMachines;
using UnityEngine;

namespace IM.EntityIntelligence
{
    [CreateAssetMenu(menuName = "Entity Intelligence/Action State Factory")]
    public class ActionStateFactory  : StateFactory
    {
        [SerializeReferenceDropdown] [SerializeReference] private List<IEntityActionFactory> _actionFactories;
        
        public override IState Create(GameObject owner)
        {
            return new ActionState(_actionFactories.Select(x => x?.Create(owner)))
            {
                StateName = name
            };
        }
    }
}