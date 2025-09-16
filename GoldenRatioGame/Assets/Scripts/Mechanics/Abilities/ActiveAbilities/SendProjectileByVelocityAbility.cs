using System;
using IM.Values;
using UnityEngine;
using Object = UnityEngine.Object;

namespace IM.Abilities
{
    public class SendProjectileByVelocityAbility : IActiveAbility, IPreferredKeyboardBinding
    {
        private readonly GameObject _prefab;
        private readonly ICooldown _cooldown;
        private readonly Func<Vector2> _getDirection;
        private Transform _userTransform;
        
        public ICooldownReadOnly Cooldown => _cooldown;
        public bool IsBeingUsed => false;
        public bool CanUse=>!Cooldown.IsOnCooldown;
        
        public KeyCode Key { get; set; } = KeyCode.Q;

        public SendProjectileByVelocityAbility(Func<Vector2> getDirection,Transform user, GameObject prefab, float cooldown) : this(getDirection, user, prefab, new FloatCooldown(cooldown))
        {
            
        }
        
        public SendProjectileByVelocityAbility(Func<Vector2>getDirection,Transform user, GameObject prefab, ICooldown cooldown)
        {
            _getDirection = getDirection;
            _cooldown = cooldown;
            _prefab = prefab;
            _userTransform = user;
        }
        
        public bool TryUse()
        {
            if (CanUse && _cooldown.TryReset())
            {
                GameObject projectile = Object.Instantiate(_prefab);

                projectile.GetComponent<Rigidbody2D>().linearVelocity = _getDirection() * 5;
                projectile.transform.position = _userTransform.position;
                
                return true;
            }

            return false;
        }
    }
}