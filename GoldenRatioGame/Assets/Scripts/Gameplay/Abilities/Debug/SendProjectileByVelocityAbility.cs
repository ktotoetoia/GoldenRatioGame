using IM.LifeCycle;
using IM.Values;
using UnityEngine;
using UnityEngine.Pool;

namespace IM.Abilities
{
    public class SendProjectileByVelocityAbility : CastAbility, IFocusProvider
    {
        private readonly IObjectPool<GameObject> _projectileFactory;

        public float Speed { get; set; } = 5f;
        public float FocusTime { get; set; } = 1f;
        public float SpawnProjectileOffsetMagnitude { get; set; } = .5f;
        public bool UseInitial { get; set; } = false;
        
        public SendProjectileByVelocityAbility(IObjectPool<GameObject> projectileFactory, float cooldown)
            : this(projectileFactory, new FloatCooldown(cooldown)) { }

        public SendProjectileByVelocityAbility(IObjectPool<GameObject> projectileFactory, ICooldown cooldown) : base(cooldown)
        {
            _projectileFactory = projectileFactory;
        }
        
        protected override void OnWindUpComplete(UseContext context)
        {
            GameObject projectile = _projectileFactory.Get();
            Vector3 dir = GetDirection().normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            projectile.GetComponent<ITemporary>().Initialize(() => _projectileFactory.Release(projectile));
            projectile.transform.position = context.GetAnchorPosition() + dir * SpawnProjectileOffsetMagnitude;
            projectile.transform.rotation = Quaternion.Euler(0, 0, angle);
            projectile.GetComponent<Rigidbody2D>().linearVelocity = dir * Speed;
        }

        public Vector3 GetFocusPoint() => _context.InitialTargetWorldPosition;
        public Vector3 GetFocusDirection() => GetDirection().normalized;
        
        public Vector3 GetDirection() => UseInitial ? _context.GetInitialDirection() : _context.GetDirection();
    }
}