using IM.LifeCycle;
using UnityEngine;

namespace IM.EntityIntelligence
{
    public abstract class ConditionFactory : ScriptableObject, IFactory<ICondition, GameObject>
    {
        public abstract ICondition Create(GameObject param1);
    }
}