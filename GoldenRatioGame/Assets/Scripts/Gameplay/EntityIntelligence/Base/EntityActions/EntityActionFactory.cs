using IM.LifeCycle;
using UnityEngine;

namespace IM.EntityIntelligence
{
    public abstract class EntityActionFactory : ScriptableObject, IFactory<IEntityAction,GameObject>
    {
        public abstract IEntityAction Create(GameObject param1);
    }
}