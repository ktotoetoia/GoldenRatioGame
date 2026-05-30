using UnityEngine;

namespace IM.EntityIntelligence
{
    public abstract class ConditionFactory : ScriptableObject, IConditionFactory
    {
        public abstract ICondition Create(GameObject param1);
    }
}