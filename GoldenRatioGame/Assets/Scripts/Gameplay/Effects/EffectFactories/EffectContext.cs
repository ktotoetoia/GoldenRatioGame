using UnityEngine;

namespace IM.Effects
{
    public class EffectContext : IEffectContext
    {
        public GameObject Instigator { get; }
        public GameObject Target { get; }
        
        public EffectContext(GameObject instigator, GameObject target)
        {
            Instigator = instigator;
            Target = target;
        }
    }
}