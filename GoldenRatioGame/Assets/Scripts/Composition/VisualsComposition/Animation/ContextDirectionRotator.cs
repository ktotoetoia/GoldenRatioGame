using IM.Abilities;
using UnityEngine;

namespace IM.Visuals
{
    public sealed class ContextDirectionRotator : MonoBehaviour, IRequireModuleVisualObjectInitialization
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _offsetAngle;
        [SerializeField] private bool _animate;
        private IAbilityContainer _abilityContainer;
        
        public void OnModuleVisualObjectInitialized(IModuleVisualObject moduleVisualObject)
        {
            _abilityContainer = moduleVisualObject.Owner.Extensions.Get<IAbilityContainer>();
        }
        
        private void Update()
        {
            if (!_animate || _abilityContainer?.Ability is not IFocusPointProvider focusPointProvider || !_target)
            {
                _target.rotation = Quaternion.identity;
                return;
            }

            RotateTowardsContext(focusPointProvider);
        }

        private void RotateTowardsContext( IFocusPointProvider focusPointProvider )
        {
            Vector3 direction = focusPointProvider.GetFocusDirection().normalized;

            if (direction.sqrMagnitude < 0.0001f)
                return;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + _offsetAngle;

            _target.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        private void OnDisable()
        {
            _animate = false;
        }
    }
}