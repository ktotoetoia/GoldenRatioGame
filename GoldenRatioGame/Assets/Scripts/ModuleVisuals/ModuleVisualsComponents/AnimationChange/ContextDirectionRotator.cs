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
        private IFocusPointProvider _focusPointProvider;
        
        public void OnModuleVisualObjectInitialized(IModuleVisualObject moduleVisualObject)
        {
            IAbilityReadOnly ability = moduleVisualObject.Owner.Extensions.Get<IAbilityExtension>()?.Ability;
            
            _focusPointProvider = ability as IFocusPointProvider;
            
        }
        
        private void Update()
        {
            if (!_animate || _focusPointProvider == null || !_target)
            {
                _target.rotation = Quaternion.identity;
                return;
            }

            RotateTowardsContext();
        }

        private void RotateTowardsContext()
        {
            Vector3 direction = _focusPointProvider.GetFocusDirection().normalized;

            if (direction.sqrMagnitude < 0.0001f)
                return;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + _offsetAngle;

            _target.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}