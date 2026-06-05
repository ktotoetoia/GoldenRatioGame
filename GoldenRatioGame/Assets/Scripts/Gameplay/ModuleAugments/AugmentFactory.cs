using UnityEngine;

namespace IM.Augments
{
    public abstract class AugmentFactory : ScriptableObject, IAugmentFactory
    {
        public abstract IAugment Create(IAugmentContext context);
        public abstract object Save(IAugment augment);
        public abstract IAugment Restore(object saved,IAugmentContext context);
    }
}