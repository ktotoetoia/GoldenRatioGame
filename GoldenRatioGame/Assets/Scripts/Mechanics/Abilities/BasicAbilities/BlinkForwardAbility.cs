using System;
using IM.Entities;
using IM.Values;
using UnityEngine;

namespace IM.Abilities
{
    public class BlinkForwardAbility : IAbility, IPreferredKeyboardBinding
    {
        private readonly ICooldown _cooldown;
        private readonly Func<Vector2> _getLookDirection;
        
        public bool IsBeingUsed => false;
        public bool CanUse =>!Cooldown.IsOnCooldown && Rigidbody != null;
        public ICooldownReadOnly Cooldown => _cooldown;
        public float Range { get; set; } = 2f;
        public KeyCode Key { get; set; } = KeyCode.E;
        public Rigidbody2D Rigidbody { get; set; }
        
        public BlinkForwardAbility(Func<Vector2> getLookDirection, Rigidbody2D rigidbody,float cooldownInSeconds) : this(getLookDirection, rigidbody,new FloatCooldown(cooldownInSeconds))
        {
            
        }
        
        public BlinkForwardAbility(Func<Vector2> getLookDirection, Rigidbody2D rigidbody, ICooldown cooldown)
        {
            _getLookDirection = getLookDirection;
            Rigidbody = rigidbody;
            _cooldown = cooldown;
        }
        
        public bool TryUse()
        {
            if (!CanUse || !_cooldown.TryReset()) return false;
            
            Vector2 offset = (_getLookDirection() - Rigidbody.position).normalized * Range;
                
            Rigidbody.MovePosition(Rigidbody.position + offset);
            
            return true;
        }
    }
}