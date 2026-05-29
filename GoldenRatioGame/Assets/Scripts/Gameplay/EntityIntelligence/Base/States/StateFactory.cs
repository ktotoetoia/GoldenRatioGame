using IM.LifeCycle;
using IM.StateMachines;
using UnityEngine;

namespace IM.EntityIntelligence
{
    public abstract class StateFactory : ScriptableObject, IFactory<IState,GameObject>
    {
        public abstract IState Create(GameObject owner);
    }
}