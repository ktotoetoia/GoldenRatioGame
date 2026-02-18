using IM.Abilities;
using IM.Modules;
using UnityEngine;

namespace IM.Visuals
{
    public sealed class ContextDirectionRotator : MonoBehaviour, IRequireModuleVisualObjectInitialization
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _offsetAngle;
        [SerializeField] private bool _animate;
        [SerializeField] private bool _setToZero;
        private IUseContextAbility _ability;

        public void OnModuleVisualObjectInitialized(IModuleVisualObject moduleVisualObject)
        {
            _ability = moduleVisualObject
                .Owner
                .Extensions
                .GetExtension<IAbilityExtension>()
                ?.Ability as IUseContextAbility;
        }
        
        private void Update()
        {
            if (!_animate || _ability == null || !_target)
            {
                _target.rotation = Quaternion.identity;
                return;
            }

            RotateTowardsContext();
        }

        private void RotateTowardsContext()
        {
            var ctx = _ability.LastUsedContext;

            Vector2 direction = (ctx.TargetWorldPosition - ctx.EntityPosition).normalized;

            if (direction.sqrMagnitude < 0.0001f)
                return;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + _offsetAngle;

            _target.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}