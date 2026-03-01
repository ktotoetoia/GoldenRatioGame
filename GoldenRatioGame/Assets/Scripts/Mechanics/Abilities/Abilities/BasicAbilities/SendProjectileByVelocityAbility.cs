using System;
using IM.Values;
using UnityEngine;
using UnityEngine.Pool;

namespace IM.Abilities
{
    public class SendProjectileByVelocityAbility : ICastAbility, IRequireAbilityUseContext, IFocusPointProvider, IAbilityEvents
    {
        private readonly IObjectPool<GameObject> _projectileFactory;
        private readonly ICooldown _cooldown;
        private readonly ICooldown _sCooldown;
        private readonly IPositionProvider _positionProvider;
        private AbilityUseContext _context;
        
        public bool CanUse => !Cooldown.IsOnCooldown;
        public ICooldownReadOnly Cooldown => _cooldown;
        public float Speed { get; set; } = 5f;
        public AbilityUseContext LastUsedContext => _context;
        public float FocusTime { get; set; } = 0.5f;
        
        public event Action<AbilityUseContext> AbilityStarted;
        public event Action<AbilityUseContext> AbilityFinished;

        public SendProjectileByVelocityAbility(IObjectPool<GameObject> projectileFactory,IPositionProvider positionProvider, float cooldown) : this(projectileFactory,positionProvider, new FloatCooldown(cooldown))
        {
            
        }
        
        public SendProjectileByVelocityAbility(IObjectPool<GameObject> projectileFactory,IPositionProvider positionProvider, ICooldown cooldown)
        {
            _projectileFactory = projectileFactory;
            _cooldown = cooldown;
            _positionProvider = positionProvider;
            _sCooldown = new FloatCooldown(0);
        }

        public bool TryCast(out ICastInfo castInfo)
        {
            castInfo = new CastInfo();
            
            if (!CanUse || !_cooldown.TryReset()) return false;
            
            GameObject projectile = _projectileFactory.Get();
            Vector3 dir = _context.GetDirection().normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            
            projectile.GetComponent<ITemporary>().Initialize(()=> _projectileFactory.Release(projectile.gameObject));
            
            projectile.transform.position = _positionProvider.GetPosition();
            projectile.transform.rotation = Quaternion.Euler(0, 0, angle);
            projectile.GetComponent<Rigidbody2D>().linearVelocity = dir * Speed;
            _sCooldown.ForceReset();
            
            AbilityStarted?.Invoke(_context);
            AbilityFinished?.Invoke(_context);
            
            return true;
        }
        
        Vector3 IFocusPointProvider.GetFocusPoint() => _context.TargetWorldPosition;
        public Vector3 GetFocusDirection() => (_context.TargetWorldPosition - _context.EntityPosition).normalized;
        public void UpdateAbilityUseContext(AbilityUseContext context) => _context = context;
    }
}