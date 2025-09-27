using System;
using IM.Values;
using UnityEngine;

namespace IM.Abilities
{
    public class BlinkForwardAbility : IAbility, IPreferredKeyboardBinding
    {
        private readonly ICooldown _cooldown;
        
        public bool IsBeingUsed => false;
        public bool CanUse =>!Cooldown.IsOnCooldown && Rigidbody is not null && GetLookDirection != null;
        public ICooldownReadOnly Cooldown => _cooldown;
        public float Range { get; set; } = 2f;
        public KeyCode Key { get; set; } = KeyCode.E;
        public Rigidbody2D Rigidbody { get; set; }
        public Func<Vector2> GetLookDirection { get; set; }

        public BlinkForwardAbility(ICooldown cooldown)
        {
            _cooldown = cooldown;
        }
        
        public BlinkForwardAbility(Func<Vector2> getLookDirection, Rigidbody2D rigidbody,float cooldownInSeconds) : this(getLookDirection, rigidbody,new FloatCooldown(cooldownInSeconds))
        {
            
        }
        
        public BlinkForwardAbility(Func<Vector2> getLookDirection, Rigidbody2D rigidbody, ICooldown cooldown)
        {
            GetLookDirection = getLookDirection;
            Rigidbody = rigidbody;
            _cooldown = cooldown;
        }
        
        public bool TryUse()
        {
            if (!CanUse || !_cooldown.TryReset()) return false;
            
            Vector2 offset = (GetLookDirection() - Rigidbody.position).normalized * Range;
                
            Rigidbody.MovePosition(Rigidbody.position + offset);
            
            return true;
        }
    }
}