using IM.LifeCycle;
using IM.StateMachines;
using UnityEngine;

namespace IM.EntityIntelligence
{
    public abstract class TransitionFactory : ScriptableObject, IFactory<ITransition, GameObject,IState, IState>
    {
        public abstract ITransition Create(GameObject param1, IState from, IState to);
    }
}