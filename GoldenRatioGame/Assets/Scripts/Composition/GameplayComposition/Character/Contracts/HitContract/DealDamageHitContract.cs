using IM.Health;
using UnityEngine;

namespace IM
{
    public class DealDamageHitContract : HitContractBase
    {
        private IDamageDealer _damageDealer;
    
        protected override void Awake()
        {
            base.Awake();
            _damageDealer = GetComponent<IDamageDealer>();
        }

        protected override void ProcessHit(GameObject target)
        {
            if (!target.TryGetComponent(out IHealth health)) return;
            
            _damageDealer.DealDamage(health);
        }
    }
}