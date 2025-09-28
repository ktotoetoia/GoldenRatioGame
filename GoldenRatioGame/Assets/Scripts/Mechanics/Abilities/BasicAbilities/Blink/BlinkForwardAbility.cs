using IM.Values;
using UnityEngine;

namespace IM.Abilities
{
    public class BlinkForwardAbility : IAbility, IPreferredKeyboardBinding
    {
        private readonly ICooldown _cooldown;
        
        public bool IsBeingUsed => false;
        public bool CanUse =>!Cooldown.IsOnCooldown && Rigidbody is not null && DirectionProvider != null;
        public ICooldownReadOnly Cooldown => _cooldown;
        public float Range { get; set; } = 2f;
        public KeyCode Key { get; set; } = KeyCode.E;
        public Rigidbody2D Rigidbody { get; set; }
        public IDirectionProvider DirectionProvider { get; set; }

        public BlinkForwardAbility(ICooldown cooldown)
        {
            _cooldown = cooldown;
        }
        
        public BlinkForwardAbility(IDirectionProvider directionProvider, Rigidbody2D rigidbody,float cooldownInSeconds) : this(directionProvider, rigidbody,new FloatCooldown(cooldownInSeconds))
        {
            
        }
        
        public BlinkForwardAbility(IDirectionProvider directionProvider, Rigidbody2D rigidbody, ICooldown cooldown)
        {
            DirectionProvider = directionProvider;
            Rigidbody = rigidbody;
            _cooldown = cooldown;
        }
        
        public bool TryUse()
        {
            if (!CanUse || !_cooldown.TryReset()) return false;
            
            Vector2 offset = DirectionProvider.GetDirection().normalized * Range;
                
            Rigidbody.MovePosition(Rigidbody.position + offset);
            
            return true;
        }
    }
}