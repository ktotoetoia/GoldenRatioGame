using System;
using IM.Values;
using UnityEngine;
using UnityEngine.Pool;

namespace IM.Abilities
{
    public class SendProjectileByVelocityAbility : IAbility,IRequireAbilityUseContext
    {
        private readonly IObjectPool<GameObject> _projectileFactory;
        private readonly ICooldown _cooldown;
        private readonly IPositionProvider _getPosition;
        private AbilityUseContext _context;
        
        public bool IsBeingUsed => false;
        public bool CanUse => !Cooldown.IsOnCooldown;
        public ICooldownReadOnly Cooldown => _cooldown;
        public float Speed { get; set; } = 5f;
        
        public SendProjectileByVelocityAbility(IObjectPool<GameObject> projectileFactory,IPositionProvider getPosition, float cooldown) : this(projectileFactory,getPosition, new FloatCooldown(cooldown))
        {
            
        }
        
        public SendProjectileByVelocityAbility(IObjectPool<GameObject> projectileFactory,IPositionProvider getPosition, ICooldown cooldown)
        {
            _projectileFactory = projectileFactory;
            _cooldown = cooldown;
            _getPosition = getPosition;
        }

        public bool TryUse()
        {
            if (!CanUse || !_cooldown.TryReset()) return false;
            
            GameObject projectile = _projectileFactory.Get();
            Vector3 dir = (_context.TargetWorldPosition - _context.EntityPosition).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            projectile.transform.position = _getPosition.GetPosition();
            projectile.transform.rotation = Quaternion.Euler(0, 0, angle);
            
            projectile.GetComponent<Rigidbody2D>().linearVelocity = dir * Speed;
            projectile.GetComponent<IProjectile>().Initialize(()=> _projectileFactory.Release(projectile.gameObject));
            
            return true;
        }
        
        public void UpdateAbilityUseContext(AbilityUseContext context) => _context = context;
    }
}