using UnityEngine;

namespace IM.Augments
{
    public class AugmentContext : IAugmentContext
    {
        public GameObject Target { get; }

        public AugmentContext(GameObject target)
        {
            Target = target;
        }
    }
}