using IM.LifeCycle;
using IM.StateMachines;
using UnityEngine;

namespace IM.EntityIntelligence
{
    public abstract class StateMachineFactory : ScriptableObject, IFactory<IStateMachine, GameObject>
    {
        public abstract IStateMachine Create(GameObject param1);
    }
}