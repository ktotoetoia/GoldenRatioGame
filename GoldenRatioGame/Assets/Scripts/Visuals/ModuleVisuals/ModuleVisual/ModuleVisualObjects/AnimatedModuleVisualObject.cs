using System.Collections.Generic;
using UnityEngine;

namespace IM.Visuals
{
    [DisallowMultipleComponent]
    public class AnimatedModuleVisualObject : ModuleVisualObject, IAnimatedModuleVisualObject
    {
        private Animator _animator;
        
        public bool IsAnimating { get; set; } = true;
        public IEnumerable<IAnimationChange> AnimationChanges { get; set; }
        
        protected override void Awake()
        {
            _animator = GetComponent<Animator>();
            base.Awake();
        }

        private void Update()
        {
            if (AnimationChanges == null || !IsAnimating || !Visible || !_animator || !_animator.isActiveAndEnabled ||
                !_animator.runtimeAnimatorController) return;
            
            foreach (IAnimationChange animationChange in AnimationChanges)
            {
                animationChange.ApplyToAnimator(_animator);
            }
        }
    }
}