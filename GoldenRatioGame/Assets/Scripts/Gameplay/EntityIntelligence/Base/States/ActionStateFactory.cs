using System.Collections.Generic;
using System.Linq;
using IM.StateMachines;
using UnityEngine;

namespace IM.EntityIntelligence
{
    [CreateAssetMenu(menuName = "Entity Intelligence/Action State Factory")]
    public class ActionStateFactory  : StateFactory
    {
        [SerializeField] private List<EntityActionFactory> _actionFactories;
        
        public override IState Create(GameObject owner) => new ActionState(_actionFactories.Select(x => x.Create(owner)));
    }
}