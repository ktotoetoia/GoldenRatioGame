using System;
using IM.Values;
using UnityEngine;
using Object = UnityEngine.Object;

namespace IM.Abilities
{
    public class SendProjectileByVelocityAbility : IAbility, IPreferredKeyboardBinding
    {
        private readonly ICooldown _cooldown;
        private readonly Func<Vector2> _getDirection;
        private readonly GameObject _prefab;
        
        public bool IsBeingUsed => false;
        public bool CanUse=>!Cooldown.IsOnCooldown && User;
        public ICooldownReadOnly Cooldown => _cooldown;
        public KeyCode Key { get; set; } = KeyCode.Q;
        public Transform User { get; set; }
        public float Speed { get; set; } = 5f;
        
        public SendProjectileByVelocityAbility(Func<Vector2> getDirection,Transform user, GameObject prefab, float cooldown) : this(getDirection, user, prefab, new FloatCooldown(cooldown))
        {
            
        }
        
        public SendProjectileByVelocityAbility(Func<Vector2>getDirection,Transform user, GameObject prefab, ICooldown cooldown)
        {
            _getDirection = getDirection;
            _cooldown = cooldown;
            _prefab = prefab;
            User = user;
        }
        
        public bool TryUse()
        {
            if (CanUse && _cooldown.TryReset())
            {
                GameObject projectile = Object.Instantiate(_prefab);
                
                projectile.GetComponent<Rigidbody2D>().linearVelocity = (_getDirection() - (Vector2)User.position).normalized * Speed;
                projectile.transform.position = User.position;
                
                return true;
            }

            return false;
        }
    }
}