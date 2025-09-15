using System;
using IM.Economy;
using UnityEngine;

namespace IM.Abilities
{
    public class BlinkForwardAbility : IAbility
    {
        private ICooldown _cooldown;
        public Func<Vector2> GetDirection { get; }
        public Func<Transform> GetTarget { get;  }
        public ICooldownReadOnly Cooldown => _cooldown;

        public BlinkForwardAbility(Func<Vector2> getDirection, Func<Transform> target,float cooldownInSeconds) : this(getDirection, target,new FloatCooldown(cooldownInSeconds))
        {
            
        }
        
        public BlinkForwardAbility(Func<Vector2> getDirection,Func<Transform> target, ICooldown cooldown)
        {
            GetDirection = getDirection;
            GetTarget = target;
            _cooldown = cooldown;
        }
        
        public bool TryUse()
        {
            if (CanUse() && _cooldown.TryReset())
            {
                Transform target = GetTarget();
                Vector3 direction = GetDirection();
                target.position += direction;
                
                return true;
            }

            return false;
        }

        public bool CanUse()
        {
            return !Cooldown.IsOnCooldown;
        }
    }
}