using System;
using IM.Abilities;
using IM.Modules;
using UnityEngine;

namespace IM.Visuals
{
    public class AnimateOnAbilityUse : MonoBehaviour, IAnimationChange
    {
        private Vector2 _velocity;
        private bool _used;
        private IUseContextAbility _contextAbility;

        [field: SerializeField] public string ParameterName { get; private set; }
        public ParameterType ParameterType => ParameterType.Trigger;
        
        private void Awake()
        {
            if (GetComponent<IAbilityExtension>().Ability is not IUseContextAbility useContextAbility)
                throw new Exception("ability must implement IUseContextAbility");
            
            useContextAbility.OnAbilityUsed += OnAbilityUsed;
            _contextAbility = useContextAbility;
        }

        public void ApplyToAnimator(Animator animator)
        {
            if (!animator.isActiveAndEnabled || !_used) return;
            
            animator.SetTrigger(ParameterName);
            _used = false;
        }

        private void OnAbilityUsed(AbilityUseContext context)
        {
            _used = true;
        }

        private void OnDestroy()
        {
            if (_contextAbility != null) _contextAbility.OnAbilityUsed -= OnAbilityUsed;
        }
    }
}