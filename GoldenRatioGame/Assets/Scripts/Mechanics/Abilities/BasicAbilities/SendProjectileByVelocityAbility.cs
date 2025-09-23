using System;
using IM.Base;
using IM.Values;
using UnityEngine;

namespace IM.Abilities
{
    public class SendProjectileByVelocityAbility : IAbility, IPreferredKeyboardBinding
    {
        private readonly IFactory<GameObject> _projectileFactory;
        private readonly ICooldown _cooldown;
        private readonly Func<Vector2> _getDirection;
        
        public bool IsBeingUsed => false;
        public bool CanUse=>!Cooldown.IsOnCooldown;
        public ICooldownReadOnly Cooldown => _cooldown;
        public KeyCode Key { get; set; } = KeyCode.Q;
        public float Speed { get; set; } = 5f;
        
        public SendProjectileByVelocityAbility(Func<Vector2> getDirection,IFactory<GameObject> projectileFactory, float cooldown) : this(getDirection, projectileFactory, new FloatCooldown(cooldown))
        {
            
        }
        
        public SendProjectileByVelocityAbility(Func<Vector2>getDirection,IFactory<GameObject> projectileFactory, ICooldown cooldown)
        {
            _getDirection = getDirection;
            _projectileFactory = projectileFactory;
            _cooldown = cooldown;
        }
        
        public bool TryUse()
        {
            if (!CanUse || !_cooldown.TryReset()) return false;
            
            GameObject projectile = _projectileFactory.Create();
            
            projectile.GetComponent<Rigidbody2D>().linearVelocity = (_getDirection() - (Vector2)projectile.transform.position).normalized * Speed;
                
            return true;

        }
    }
}