using System;
using System.Collections.Generic;
using System.Linq;
using IM.StateMachines;
using UnityEngine;

namespace IM.EntityIntelligence
{
    [CreateAssetMenu(menuName = "Entity Intelligence/Default State Machine Factory")]
    public class DefaultStateMachineFactory : StateMachineFactory
    {
        [SerializeField] private List<StateFactory> _stateFactories;
        [SerializeField] private List<TransitionInfo> _transitionInfos;
        
        public override IStateMachine Create(GameObject param1)
        {
            List<IState> states = _stateFactories.Select(x => x.Create(param1)).ToList();

            foreach (TransitionInfo transitionInfo in _transitionInfos)
            {
                IState from = states[transitionInfo.FromIndex];
                IState to = states[transitionInfo.ToIndex];
                ITransition transition = transitionInfo.Factory.Create(param1,from,to);
                
                from.AddTransition(transition);
            }
            
            return new StateMachine(states[0]);
        }

        [Serializable]
        private class TransitionInfo
        {
            [field:SerializeField] public TransitionFactory Factory { get; private set; }
            [field:SerializeField] public int FromIndex{ get; private set; }
            [field:SerializeField] public int ToIndex{ get; private set; }
        }
    }
}