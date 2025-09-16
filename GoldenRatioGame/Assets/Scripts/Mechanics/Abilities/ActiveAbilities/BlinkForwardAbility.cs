using System;
using IM.Values;
using UnityEngine;

namespace IM.Abilities
{
    public class BlinkForwardAbility : IActiveAbility, IPreferredKeyboardBinding
    {
        private readonly ICooldown _cooldown;
        private readonly Transform _target;
        private readonly Func<Vector2> _getDirection;
        
        public KeyCode Key { get; set; } = KeyCode.E;
        public bool IsBeingUsed => false;
        public bool CanUse =>!Cooldown.IsOnCooldown;
        public ICooldownReadOnly Cooldown => _cooldown;

        public BlinkForwardAbility(Func<Vector2> getDirection, Transform target,float cooldownInSeconds) : this(getDirection, target,new FloatCooldown(cooldownInSeconds))
        {
            
        }
        
        public BlinkForwardAbility(Func<Vector2> getDirection,Transform target, ICooldown cooldown)
        {
            _getDirection = getDirection;
            _target = target;
            _cooldown = cooldown;
        }
        
        public bool TryUse()
        {
            if (CanUse && _cooldown.TryReset())
            {
                Transform target = _target;
                Vector3 direction = _getDirection();
                target.position += direction;
                
                return true;
            }

            return false;
        }
    }
}